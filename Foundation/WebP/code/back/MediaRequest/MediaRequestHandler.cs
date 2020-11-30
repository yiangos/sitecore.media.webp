using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Yiangos.Foundation.MediaWebP.MediaRequest
{
    // Sitecore.Resources.Media.MediaRequestHandler
    using Sitecore;
    using Sitecore.Configuration;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Events;
    using Sitecore.Globalization;
    using Sitecore.Pipelines.GetResponseCacheHeaders;
    using Sitecore.Resources;
    using Sitecore.Resources.Media;
    using Sitecore.Resources.Media.Streaming;
    using Sitecore.Security.Accounts;
    using Sitecore.SecurityModel;
    using Sitecore.Sites;
    using Sitecore.Text;
    using Sitecore.Web;
    using Sitecore.Web.Authentication;
    using System;
    using System.Collections.Generic;
    using System.Web;

    /// <summary>
    /// MediaRequestHandler class
    /// </summary>
    public class MediaRequestHandler : Sitecore.Resources.Media.MediaRequestHandler
    {
        /// <summary>
        /// Executes the process request event.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="media">
        /// The Sitecore media.
        /// </param>
        /// <returns>
        /// The <see cref="T:System.Boolean" /> that indicates whether request has been processed.
        /// </returns>
        protected override bool DoProcessRequest(HttpContext context, MediaRequest request, Media media)
        {
            Assert.ArgumentNotNull(context, "context");
            Assert.ArgumentNotNull(request, "request");
            Assert.ArgumentNotNull(media, "media");
            SetWebpOptions(request, context);
            return base.DoProcessRequest(context, request, media);
        }

        private bool AcceptWebP(HttpContext context)
        {
            Sitecore.Diagnostics.Log.Info("::MEDIA:: webp enabled:" + (context?.Request.AcceptTypes != null && (context.Request.AcceptTypes).Contains("image/webp")).ToString() + ",  user Agent:" + context.Request.UserAgent + ", raw accept-types:" + string.Join(", ", context.Request.AcceptTypes), this);
            return context?.Request.AcceptTypes != null && (context.Request.AcceptTypes).Contains("image/webp");
        }

        /// <summary>
        /// Processes the accept webp option for dynamically scaled images
        /// </summary>
        /// <param name="context">The HttpContext.</param>
        private void SetWebpOptions(MediaRequest request, HttpContext context)
        {
            Assert.ArgumentNotNull(request, "request");
            if (AcceptWebP(context))
            {
                request.Options.CustomOptions["webp"] = "1";
            }
        }
    }
}