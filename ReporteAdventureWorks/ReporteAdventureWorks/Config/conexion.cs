using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReporteAdventureWorks.Config
{
    internal class conexion
    {
        private readonly string cadenaConexion =
            "Server=(local);database=AdventureWorks2022;uid=sa;pwd=123456";

        public SqlConnection obtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }
    }
}
