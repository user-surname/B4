la tabla **USUARIOS** almacena la información de los usuarios:

*   **id** → identificador único de cada usuario.
    
*   **email** → correo electrónico, único y obligatorio.
    
*   **hashed\_password** → contraseña encriptada, obligatoria.
    
*   **role** → rol del usuario (por defecto user).
    
*   **created\_at** → fecha de creación, se asigna automáticamente.
    
*   **updated\_at** → fecha de última actualización, se actualiza automáticamente cuando se modifica el registro.
    

En resumen, gestiona usuarios, sus credenciales y su rol, con control de fechas de creación y actualización.