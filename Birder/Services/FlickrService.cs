using Birder.Data.Model;
using FlickrNet;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Services
{
    public interface IFlickrService
    {
        PhotoCollection GetFlickrPhotoCollection(string queryString);
        string GetThumbnailUrl(string queryString);
    }

    public class FlickrService : IFlickrService
    {
        private readonly IConfiguration _config;

        public FlickrService(IConfiguration config)
        {
            _config = config;
        }

        public string GetThumbnailUrl(string queryString)
        {
            Flickr flickr = new Flickr(_config["Flickr:FlickrApiKey"], _config["Flickr:FlickrSecret"]);
            {
                var options = new PhotoSearchOptions
                {
                    Text = queryString,
                    Page = 1,
                    PerPage = 1,
                    Extras = PhotoSearchExtras.SmallUrl,
                    SafeSearch = SafetyLevel.Safe,
                    MediaType = MediaType.Photos
                };
                PhotoCollection photos = flickr.PhotosSearch(options);
                return photos.FirstOrDefault().LargeSquareThumbnailUrl;
            }


        }

        public PhotoCollection GetFlickrPhotoCollection(string queryString)
        {
            // ToDo: Make asynchronous, if possible...
            // ToDo: Implement disposable to use the using statement...
            Flickr flickr = new Flickr(_config["FlickrApiKey"], _config["FlickrSecret"]);
            {
                var options = new PhotoSearchOptions
                {
                    Text = queryString,
                    Extras = PhotoSearchExtras.AllUrls,
                    SafeSearch = SafetyLevel.Safe,
                    MediaType = MediaType.Photos
                };
                return flickr.PhotosSearch(options);
            }
        }
    }
}
