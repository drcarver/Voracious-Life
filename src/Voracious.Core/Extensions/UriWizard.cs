using System;
using System.IO;


namespace Voracious.Core.Extensions
{
    class UriWizard
    {
        /// <summary>
        /// Given a URI, return an appropriate filename + extension.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static FileInfo GetUriFilename(Uri uri)
        {
            var path = uri.GetComponents(UriComponents.Path, UriFormat.Unescaped);

            // path is e.g. ebooks/14.epub.noimages
            var slashidx = path.LastIndexOf('/');

            if (slashidx >= 0)
            {
                path = path.Substring(slashidx + 1);
            }

            // path is updated to be e.g. 14.epub.noimages
            //FAIL: why would you give your epubs the wrong extension?
            //Instead of <title>.noimages.epub they are <title>.epub.noimages which makes
            //file pickers etc completely fail.
            var originalFilename = path
                .Replace(".images", string.Empty)
                .Replace(".noimages", string.Empty);

            if (originalFilename.StartsWith("https:")
                && originalFilename.Contains("gutenberg.org"))
            {
                // FAIL: NOTE: Gutenberg has some weird files that start with https://.
                // In reality, the Gutenberg server redirects these to http:
                // BUT this is not allowed in HttpClient and there's no way to 
                // force the HttpClient BaseProtocolFilter to allow the redirect.
                originalFilename = originalFilename.Replace("https://", "http://");
            }

            // path is updated to be e.g. 14.epub
            var ext = originalFilename;
            var dotidx = ext.LastIndexOf('.');

            if (dotidx >= 0)
            {
                ext = ext.Substring(dotidx);
            }

            return new FileInfo($"{originalFilename}.{ext}");
        }
    }
}
