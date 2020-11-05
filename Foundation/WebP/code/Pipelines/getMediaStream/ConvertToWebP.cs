using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Yiangos.Foundation.MediaWebP.Pipelines.getMediaStream
{
    public class ConvertToWebPProcessor
    {
        public void Process(GetMediaStreamPipelineArgs args)
        {
            Sitecore.Diagnostics.Log.Debug("::WEBP:: enabled:" + (args.Options.CustomOptions.ContainsKey("webp") || args.Options.CustomOptions["webp"] == "1").ToString(), this);
            Assert.ArgumentNotNull(args, "args");
            if (!ShouldSkip(args))
            {
                Sitecore.Diagnostics.Log.Debug("::WEBP:: converting " +args.MediaData.MediaItem.InnerItem.Paths.Path, this);
                ISupportedImageFormat format = new WebPFormat() { Quality = Sitecore.Configuration.Settings.GetIntSetting("Media.Resizing.Quality", 70) };
                MemoryStream outstream = new MemoryStream();
                using (Stream stream = args.MediaData.MediaItem.GetMediaStream())
                {
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        imageFactory.Load(stream).Format(format).Save(outstream);
                        args.OutputStream = new MediaStream(outstream, "webp", args.MediaData.MediaItem);
                        args.OutputStream.Headers.Headers["Content-Type"] = "image/webp";
                    }
                }

            }
        }
        protected bool ShouldSkip(GetMediaStreamPipelineArgs args)
        {
            MediaData mediaData = args.MediaData;
            if (!mediaData.MimeType.StartsWith("image/", StringComparison.Ordinal))
            {
                return true;
            }
            if (mediaData.MimeType.StartsWith("image/webp", StringComparison.Ordinal))
            {
                return true;
            }
            if (!args.Options.CustomOptions.ContainsKey("webp") || args.Options.CustomOptions["webp"] != "1")
            {
                return true;
            }
            if (!args.MediaData.HasContent)
            {
                return true;
            }
            return false;
        }
    }
}
