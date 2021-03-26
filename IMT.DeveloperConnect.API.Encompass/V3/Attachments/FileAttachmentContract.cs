using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.Encompass.V3.Attachments
{
    public class FileAttachmentContract
    {
        public virtual string Id { get; set; }

        public virtual string Title { get; set; }

        public virtual FileAttachmentType? AttachmentType { get; set; }

        public virtual string ObjectId { get; set; }

        public virtual string[] Sources { get; set; }

        public virtual bool? IsActive { get; set; }

        public virtual EntityReferenceContract AssignedTo { get; set; }

        public virtual long? FileSize { get; set; }

        public virtual int? Rotation { get; set; }

        public virtual bool? IsRemoved { get; set; }

        public virtual List<FileAttachmentPageContract> Pages { get; set; }

        public virtual EntityReferenceContract CreatedBy { get; set; }

        public virtual DateTime? CreatedDate { get; set; }
    }

    public class FileAttachmentPageContract
    {
        public virtual ImageEntityReferenceContract PageImage { get; set; }

        public virtual ImageEntityReferenceContract ThumbnailImage { get; set; }

        public virtual List<FileAttachmentAnnotationContract> Annotations { get; set; }

        public virtual long? FileSize { get; set; }

        public virtual int? Rotation { get; set; }

        public virtual string OriginalKey { get; set; }

    }

    public class FileAttachmentAnnotationContract
    {
        public virtual string Text { get; set; }

        public virtual EntityReferenceContract CreatedBy { get; set; }

        public virtual DateTime? CreatedDate { get; set; }

        public FileAttachmentAnnotationVisibilityType Visibility { get; set; }

        public virtual int? Left { get; set; }

        public virtual int? Top { get; set; }

        public virtual int? Width { get; set; }

        public virtual int? Height { get; set; }
    }

    public class ImageEntityReferenceContract
    {
        public virtual int? Height { get; set; }

        public virtual int? Width { get; set; }

        public virtual int? DpiX { get; set; }

        public virtual int? DpiY { get; set; }

        public virtual string ZipKey { get; set; }

        public virtual string ImageKey { get; set; }

        public virtual string OriginalKey { get; set; }
    }

    public enum FileAttachmentAnnotationVisibilityType
    {
        /// <summary>
        /// Viewable by everyone and is sent as part of documents
        /// </summary>
        Public = 0,

        /// <summary>
        /// Viewable by any person with Encompass permissions to view annotation
        /// </summary>
        Internal = 1,

        /// <summary>
        /// Viewable by the user who added it
        /// </summary>
        Personal = 2
    }

    public enum FileAttachmentType
    {
        Native = 0,
        Image = 1,
        Background = 2,
        Cloud = 3
    }
}
