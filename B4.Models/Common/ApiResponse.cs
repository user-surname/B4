using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Common
{
    /// <summary>
    /// Envelope uniforme para todas las respuestas de la API.
    /// Reemplaza los objetos anónimos { coderror, msg, data... } dispersos en los controllers.
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Success { get; private set; }
        public int CodeError { get; private set; }
        public string Msg { get; private set; }
        public string Ts { get; private set; }
        public long ExecTimeMs { get; private set; }
        public int Count { get; private set; }
        public T? Data { get; private set; }

        private ApiResponse(bool success, int codeError, string msg, T? data, long execTimeMs = 0)
        {
            Success = success;
            CodeError = codeError;
            Msg = msg;
            Data = data;
            Ts = DateTime.UtcNow.ToString("o");
            ExecTimeMs = execTimeMs;
            Count = data is System.Collections.IEnumerable e && data is not string
                ? e.Cast<object>().Count()
                : data is not null ? 1 : 0;
        }

        // ── Fábrica de respuestas OK ─────────────────────────────────────────────

        public static ApiResponse<T> Ok(T data, long execTimeMs = 0)
            => new(true, 0, string.Empty, data, execTimeMs);

        public static ApiResponse<T> Created(T data)
            => new(true, 0, "Recurso creado correctamente", data);

        // ── Fábrica de respuestas de error ───────────────────────────────────────

        public static ApiResponse<T> NotFound(string msg)
            => new(false, 404, msg, default);

        public static ApiResponse<T> BadRequest(string msg)
            => new(false, 400, msg, default);

        public static ApiResponse<T> Unauthorized(string msg = "No estás autorizado")
            => new(false, 401, msg, default);

        public static ApiResponse<T> ServerError(string msg = "Error interno del servidor")
            => new(false, 500, msg, default);
    }
}
