using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreDynamoS3AML.Models
{
    [DynamoDBTable("useraws")]
    public class Usuario
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("iduser")]
        public int IdUsuario { get; set; }
        [DynamoDBProperty("nombre")]
        public String Nombre { get; set; }
        [DynamoDBProperty("descripcion")]
        public String Descripcion { get; set; }
        [DynamoDBProperty("fechaalta")]
        public String FechaAlta { get; set; }
        //FUNCIONA V1
        [DynamoDBProperty("fotos")]
        public Foto Fotos { get; set; }

        //[DynamoDBProperty("fotos")]
        //public List<Foto> Fotos { get; set; }


    }
}
