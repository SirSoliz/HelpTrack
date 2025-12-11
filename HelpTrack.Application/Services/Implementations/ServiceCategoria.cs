using AutoMapper;
using HelpTrack.Application.DTOs;
using HelpTrack.Application.Services.Interfaces;
using HelpTrack.Infraestructure.Models;
using HelpTrack.Infraestructure.Repository.Implementations;
using HelpTrack.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace HelpTrack.Application.Services.Implementations
{
    public class ServiceCategoria : IServiceCategoria
    {
        private readonly IRepositoryCategoria _repository;
        private readonly IMapper _mapper;
        private readonly IRepositorySla _repositorySla;
        private readonly IRepositoryEtiqueta _repositoryEtiqueta;
        private readonly IRepositoryEspecialidad _repositoryEspecialidad;

        public ServiceCategoria(IRepositoryCategoria repository, IMapper mapper, IRepositorySla repositorySla,
            IRepositoryEtiqueta repositoryEtiqueta, IRepositoryEspecialidad repositoryEspecialidad)
        {
            _repository = repository;
            _mapper = mapper;
            _repositorySla = repositorySla;
            _repositoryEtiqueta = repositoryEtiqueta;
            _repositoryEspecialidad = repositoryEspecialidad;
        }
        public async Task<int> AddAsync(CategoriaDTO dto)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var entity = _mapper.Map<Categorias>(dto);

                    // Cargar y asignar SLA
                    var sla = await _repositorySla.FindByIdAsync(dto.IdSla);
                    if (sla == null)
                    {
                        throw new ArgumentException($"No se encontró un SLA con el ID {dto.IdSla}");
                    }
                    entity.IdSlaNavigation = sla;

                    // Inicializar colecciones si son nulas
                    entity.IdEtiqueta ??= new List<Etiquetas>();
                    entity.IdEspecialidad ??= new List<Especialidades>();

                    // Agregar etiquetas seleccionadas
                    if (dto.EtiquetasSeleccionadas?.Any() == true)
                    {
                        var etiquetas = await _repositoryEtiqueta.FindByIdsAsync(dto.EtiquetasSeleccionadas);
                        foreach (var etiqueta in etiquetas)
                        {
                            entity.IdEtiqueta.Add(etiqueta);
                        }
                    }

                    // Agregar especialidades seleccionadas
                    if (dto.EspecialidadesSeleccionadas?.Any() == true)
                    {
                        var especialidades = await _repositoryEspecialidad.FindByIdsAsync(dto.EspecialidadesSeleccionadas);
                        foreach (var especialidad in especialidades)
                        {
                            entity.IdEspecialidad.Add(especialidad);
                        }
                    }

                    // Guardar la categoría con sus relaciones
                    var result = await _repository.AddAsync(entity);

                    transaction.Complete();
                    return result;
                }
                catch (Exception ex)
                {
                    // Registrar el error
                    throw new Exception("Error al guardar la categoría con sus relaciones.", ex);
                }
            }
        }
        public async Task<CategoriaDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);

            var objectMapped = _mapper.Map<CategoriaDTO>(@object);

            return objectMapped;
        }

        public async Task<ICollection<CategoriaDTO>> ListAsync()
        {            
            var list = await _repository.ListAsync();

            var collection = _mapper.Map<ICollection<CategoriaDTO>>(list);

            return collection;
        }

        public async Task UpdateAsync(int id, CategoriaDTO dto)
        {
            var @object = await _repository.FindByIdAsync(id);

            var entity = _mapper.Map(dto, @object!);

            await _repository.UpdateAsync(entity);
        }

        public async Task<CategoriaDTO> GetByIdWithDetailsAsync(int id)
        {
            var categoria = await _repository.FindByIdAsync(id);
            if (categoria == null)
                return null;

            var dto = _mapper.Map<CategoriaDTO>(categoria);
            
            // Mapear explícitamente las colecciones
            dto.Especialidades = _mapper.Map<ICollection<EspecialidadDTO>>(categoria.IdEspecialidad);
            dto.Etiquetas = _mapper.Map<ICollection<EtiquetaDTO>>(categoria.IdEtiqueta);
            
            return dto;
        }
    }
}
