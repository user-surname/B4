using B4.Api.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Tests
{
    public class PasswordHasherTests
    {
        [Fact]
        public void GenerarHashDe1234()
        {
            var hasher = new PasswordHasherService();
            var hash = hasher.HashPassword("1234");

            Console.WriteLine(hash);
            Assert.NotNull(hash);
        }
    }

}
