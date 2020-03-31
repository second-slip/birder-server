using FlickrNet;
using Microsoft.Extensions.Configuration;
using System.Linq;

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
            //Flickr flickr = new Flickr(_config["Flickr:FlickrApiKey"], _config["Flickr:FlickrSecret"]);
            //{
            //    var options = new PhotoSearchOptions
            //    {
            //        Text = queryString,
            //        Page = 1,
            //        PerPage = 1,
            //        Extras = PhotoSearchExtras.SmallUrl,
            //        SafeSearch = SafetyLevel.Safe,
            //        MediaType = MediaType.Photos
            //    };
            //    PhotoCollection photos = flickr.PhotosSearch(options);
            //    return photos.FirstOrDefault().LargeSquareThumbnailUrl;
            //}

            // temp in dev to avoid hitting the API...
            return "https://farm1.staticflickr.com/908/28167626118_f9ed3a67cf_q.png";
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
