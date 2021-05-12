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

        //[HttpPost]
        //public async Task<IActionResult> Details(int idUsuario, String titulo, IFormFile imagen)
        //{
        //    Usuario usuario = await this.serviceDynamo.FindUserAsync(idUsuario);
        //    if (usuario.Fotos == null)
        //    {
        //        usuario.Fotos = new List<Foto>();
        //    }
        //    usuario.Fotos = new List<Foto>();
        //    Foto foto = new Foto();
        //    foto.Titulo = titulo;
        //    foto.Imagen = imagen.FileName;
        //    usuario.Fotos.Add(foto);
        //    String path = await this.uploadHelper.UploadFileAsync(imagen, Folders.Images);
        //    using(FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        //    {
        //        await this.serviceAWSS3.UploadFile(stream, imagen.FileName);
        //    }
        //    return RedirectToAction("Index");
        //}


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
            //await this.serviceAWSS3.DeleteFile(usuario.Fotos.Imagen);
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        //FUNCIONA V1
        [HttpPost]
        public async Task<IActionResult> Create(Usuario usuario, String imagenyes1, String titulo1, IFormFile file)
        {
            if (imagenyes1 != null)
            {
                usuario.Fotos = new Foto();
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



        public async Task<IActionResult> Edit(int idUser)
        {
            return View(await this.serviceDynamo.FindUserAsync(idUser));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Usuario usuario)
        {
            Usuario user = await this.serviceDynamo.FindUserAsync(usuario.IdUsuario);
            usuario.FechaAlta = user.FechaAlta;
            usuario.Fotos = user.Fotos;
            await this.serviceDynamo.EditUser(usuario);
            return RedirectToAction("Index");
        }

        //public async Task<IActionResult> AddImg(int idUser)
        //{
        //    Usuario user = await this.serviceDynamo.FindUserAsync(idUser);
        //    return View(user);
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddImg(String titulo, IFormFile file, int idUser)
        //{
        //    Usuario usuario = await this.serviceDynamo.FindUserAsync(idUser);
        //    Foto foto = new Foto();
        //    foto.Titulo = titulo;
        //    foto.Imagen = file.FileName;
        //    usuario.Fotos.Add(foto);
        //    String path = await this.uploadHelper.UploadFileAsync(file, Folders.Images);
        //    using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        //    {
        //        await this.serviceAWSS3.UploadFile(stream, file.FileName);
        //    }
        //    return RedirectToAction("Index");
        //}

    }
}
