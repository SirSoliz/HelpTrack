using AutoMapper;
using HelpTrack.Application.DTOs;
using HelpTrack.Application.Services.Interfaces;
using HelpTrack.Infraestructure.Data;
using HelpTrack.Infraestructure.Models;
using HelpTrack.Infraestructure.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpTrack.Application.Services.Implementations
{
    public class ServiceRol : IServiceRol
    {
        private readonly IRepositoryRol _repositoryRol;
        private readonly IMapper _mapper;

        public ServiceRol(IRepositoryRol repositoryRol, IMapper mapper)
        {
            _repositoryRol = repositoryRol;
            _mapper = mapper;
        }

        public async Task<ICollection<RolDTO>> ListAsync()
        {
            var roles = await _repositoryRol.ListAsync();
            return _mapper.Map<ICollection<RolDTO>>(roles);
        }

        public async Task<RolDTO> FindByIdAsync(int id)
        {
            var rol = await _repositoryRol.FindByIdAsync(id);
            return _mapper.Map<RolDTO>(rol);
        }

        public async Task<int> AddAsync(RolDTO dto)
        {
            var rol = _mapper.Map<Roles>(dto);
            return await _repositoryRol.AddAsync(rol);
        }

        public async Task UpdateAsync(int id, RolDTO dto)
        {
            var rol = await _repositoryRol.FindByIdAsync(id);
            if (rol != null)
            {
                _mapper.Map(dto, rol);
                await _repositoryRol.UpdateAsync(rol);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repositoryRol.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _repositoryRol.ExistsAsync(id);
        }
    }
}
