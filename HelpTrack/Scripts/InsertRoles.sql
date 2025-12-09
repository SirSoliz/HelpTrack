-- Insertar roles básicos en la tabla Roles
-- Este script debe ejecutarse después de crear la base de datos

-- Verificar si la tabla Roles existe
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Roles')
BEGIN
    PRINT 'La tabla Roles no existe. Asegúrese de que la migración de la base de datos se ha ejecutado correctamente.'
    RETURN
END

-- Insertar roles si no existen
IF NOT EXISTS (SELECT 1 FROM Roles WHERE Nombre = 'Administrador')
BEGIN
    INSERT INTO Roles (Nombre, Descripcion)
    VALUES ('Administrador', 'Acceso completo a todas las funcionalidades del sistema')
END

IF NOT EXISTS (SELECT 1 FROM Roles WHERE Nombre = 'Técnico')
BEGIN
    INSERT INTO Roles (Nombre, Descripcion)
    VALUES ('Técnico', 'Puede gestionar tickets asignados y ver información limitada')
END

IF NOT EXISTS (SELECT 1 FROM Roles WHERE Nombre = 'Usuario')
BEGIN
    INSERT INTO Roles (Nombre, Descripcion)
    VALUES ('Usuario', 'Puede crear tickets y ver sus propios tickets')
END

IF NOT EXISTS (SELECT 1 FROM Roles WHERE Nombre = 'Supervisor')
BEGIN
    INSERT INTO Roles (Nombre, Descripcion)
    VALUES ('Supervisor', 'Puede supervisar técnicos y gestionar asignaciones')
END

PRINT 'Roles básicos insertados correctamente.'

-- Mostrar los roles insertados
SELECT * FROM Roles ORDER BY Nombre
