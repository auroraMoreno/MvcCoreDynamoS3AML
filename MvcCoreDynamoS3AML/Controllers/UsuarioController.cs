using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcCoreDynamoS3AML.Helpers;
using MvcCoreDynamoS3AML.Models;
using MvcCoreDynamoS3AML.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreDynamoS3AML.Controllers
{
    public class UsuarioController : Controller
    {
        private ServiceAWSDynamoDB serviceDynamo;
        private UploadHelper uploadHelper;
        private ServiceAWSS3 serviceAWSS3;

        public UsuarioController(ServiceAWSDynamoDB serviceDynamo, UploadHelper uploadHelper, ServiceAWSS3 serviceAWSS3)
        {
            this.serviceDynamo = serviceDynamo;
            this.uploadHelper = uploadHelper;
            this.serviceAWSS3 = serviceAWSS3;
        }

        public async Task<IActionResult> Index()
        {
            return View(await this.serviceDynamo.GetUsuariosAsync());
        }

        public async Task<IActionResult> Details(int idUser)
        {
            return View(await this.serviceDynamo.FindUserAsync(idUser));
        }


        //esto para mostrar la img: 
        public async Task<IActionResult> FileAWS(string filename)
        {
            Stream stream = await this.serviceAWSS3.GetFile(filename);
            return File(stream, "image/jpg");
        }

        public async Task<IActionResult> Delete(int idUser)
        {
            Usuario usuario = await this.serviceDynamo.FindUserAsync(idUser);
            await this.serviceDynamo.DeleteUserAsync(idUser);
            await this.serviceAWSS3.DeleteFile(usuario.Fotos.Imagen);
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Usuario usuario, String imagenyes1, String titulo1, IFormFile file)
        {
            if (imagenyes1 != null)
            {
                usuario.Fotos = new Fotos();
                String path = await this.uploadHelper.UploadFileAsync(file, Folders.Images);
                usuario.Fotos.Titulo = titulo1;
                usuario.Fotos.Imagen = file.FileName;
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    bool respuesta = await this.serviceAWSS3.UploadFile(stream, file.FileName);
                }
            }

            await this.serviceDynamo.CreateUser(usuario);
            return RedirectToAction("Index");
        }

        //aqui añadir un método que sea agregar foto 
        //le llegará el iddel usuario (desde detalles) + el iformfile 
        //idea: un form que tenga el id hidden, un input file
        //al darle, se llama al upload del servicio y tal 
    }
}
