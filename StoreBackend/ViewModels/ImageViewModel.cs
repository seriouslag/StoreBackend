using System;

namespace StoreBackend.Models
{
    public class ImageViewModel: BaseEntityViewModel
    {
        public int Id { get; set; }

        // public byte[] Content { get; set; }
        // Use File get methods to get the content
        public int Height { get; set; }
        public int Width { get; set; }
        public string ContentType { get; set; }
    }
}
