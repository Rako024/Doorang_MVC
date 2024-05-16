using Business.Exceptions;
using Business.Services.Abstracts;
using Core.Models;
using Core.RepositoryAbstracts;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concretes
{
    public class ExploreService : IExploreService
    {
        IExploreRepository _exploreRepository;
        IWebHostEnvironment _webHostEnvironment;
        public ExploreService(IExploreRepository exploreRepository, IWebHostEnvironment webHostEnvironment)
        {
            _exploreRepository = exploreRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public void CreateExplore(Explore explore)
        {
            if(explore == null)
            {
                throw new NotFoundExploreException("", "Explore is null!");
            }
            if(explore.PhotoFile == null)
            {
                throw new NotFoundExploreException("PhotoFile", "Photo File is null!");
            }
            if (!explore.PhotoFile.ContentType.Contains("image/"))
            {
                throw new PhotoFileFormatException("PhotoFile", "Photo File format is not valid!");
            }
            string path = _webHostEnvironment.WebRootPath + @"\upload\explore\" + explore.PhotoFile.FileName;
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                explore.PhotoFile.CopyTo(stream);
            }
            explore.ImgUrl = explore.PhotoFile.FileName;
            _exploreRepository.Add(explore);
            _exploreRepository.Commit();
        }

        public void DeleteExplore(int id)
        {
            var explore = _exploreRepository.Get(x => x.Id == id);
            if(explore == null)
            {
                throw new NotFoundExploreException("", "Explore is null!");
            }
            string path = _webHostEnvironment.WebRootPath + @"\upload\explore\"+explore.ImgUrl;
            FileInfo fileInfo = new FileInfo(path);
            fileInfo.Delete();
            _exploreRepository.Delete(explore);
            _exploreRepository.Commit();
        }

        public List<Explore> GetAllExplores(Func<Explore, bool>? func = null)
        {
            return _exploreRepository.GelAll(func);
        }

        public Explore GetExplore(Func<Explore, bool>? func = null)
        {
            return _exploreRepository.Get(func);
        }

        public void UpdateExplore(int id, Explore explore)
        {
            var oldExplore = _exploreRepository.Get(x => x.Id == id);
            if (oldExplore == null)
            {
                throw new NotFoundExploreException("", "Explore is null!");
            }
            string path = _webHostEnvironment.WebRootPath + @"\upload\explore\" + oldExplore.ImgUrl;
            if(explore.PhotoFile != null)
            {
                if (!explore.PhotoFile.ContentType.Contains("image/"))
                {
                    throw new PhotoFileFormatException("PhotoFile", "Photo File format is not valid!");
                }
                FileInfo fileInfo = new FileInfo(path);
                fileInfo.Delete();

                string path1 = _webHostEnvironment.WebRootPath + @"\upload\explore\" + explore.PhotoFile.FileName;
                using(FileStream stream = new FileStream(path1, FileMode.Create))
                {
                    explore.PhotoFile.CopyTo(stream);
                }
                oldExplore.ImgUrl = explore.PhotoFile.FileName;
            }
            oldExplore.Title = explore.Title;
            oldExplore.SubTitle = explore.SubTitle;
            oldExplore.Descrption = explore.Descrption;
            _exploreRepository.Commit();
        }
    }
}
