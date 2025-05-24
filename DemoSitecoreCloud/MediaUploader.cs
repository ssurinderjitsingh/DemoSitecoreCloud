using DemoSitecoreCloud.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSitecoreCloud
{
    public class MediaUploader
    {
        private readonly IGraphQLQueryBuilder _queryBuilder;
        private readonly IGraphQLExecutor _queryExecuter;

        public MediaUploader(IGraphQLQueryBuilder queryBuilder, IGraphQLExecutor queryExecutor)
        {
            _queryBuilder = queryBuilder;
            _queryExecuter = queryExecutor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="fileBytes"></param>
        /// <param name="itemPath"></param>
        /// <returns></returns>
        public async Task<bool> UploadMediaInSitecoreAsync(string itemName, byte[] fileBytes, string itemPath)
        {
            var preSignedUploadUrl = new Uri(itemPath);

            return await UploadMediaInSitecoreAsync(itemName, fileBytes, itemPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemPath"></param>
        /// <param name="localFilePath"></param>
        /// <returns></returns>
        public async Task<string> UploadMediaInSitecoreAsync(string itemPath, string localFilePath)
        {
            var query = _queryBuilder.GetMediaPresignedUrlGraphQLuery(itemPath);

            var presignedUploadUrl = await _queryExecuter.ExecuteMediaGraphQLQuery(query);

            var fileBytes = await File.ReadAllBytesAsync(localFilePath);
            using var client = new HttpClient();
            var form = new MultipartFormDataContent();
            form.Add(new ByteArrayContent(fileBytes), "file", Path.GetFileName(localFilePath));

            var uploadResponse = await client.PostAsync(presignedUploadUrl, form);
            uploadResponse.EnsureSuccessStatusCode();
            return "Upload successful";
        }
    }
}
