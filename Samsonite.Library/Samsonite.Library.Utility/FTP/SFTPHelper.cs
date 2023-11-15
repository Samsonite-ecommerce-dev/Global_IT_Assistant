using System;
using System.Collections.Generic;
using System.IO;

using Renci.SshNet;

namespace Samsonite.Library.Utility
{
    public class SFTPHelper : IDisposable
    {
        private SftpClient sftpClient;
        public SFTPHelper(string host, int port, string user, string pwd)
        {
            string[] arr = host.Split(':');
            string hostName = arr[0];
            if (arr.Length > 1) port = Int32.Parse(arr[1]);

            //初始化对象
            sftpClient = new SftpClient(host, port, user, pwd);
        }

        /// <summary>
        /// SFTP连接状态
        /// </summary>
        public bool Connected
        {
            get { return sftpClient.IsConnected; }
        }

        /// <summary>
        /// 连接SFTP
        /// </summary>
        public void Connect()
        {
            try
            {
                if (!Connected)
                {
                    sftpClient.Connect();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取SFTP文件列表
        /// </summary>
        /// <param name="remotePath">远程目录</param>
        /// <param name="fileSuffix">文件类型</param>
        /// <returns></returns>
        public List<string> GetFileList(string remotePath, string fileSuffix)
        {
            try
            {
                var fileList = new List<string>();
                var files = sftpClient.ListDirectory(remotePath);
                foreach (var fileInfo in files)
                {
                    string s = fileInfo.Name;
                    if (s.Length > (fileSuffix.Length + 1) && fileSuffix.ToUpper() == s.Substring(s.Length - fileSuffix.Length).ToUpper())
                    {
                        fileList.Add(s);
                    }
                }
                return fileList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// SFTP传输文件
        /// </summary>
        /// <param name="localFilePath">本地文件地址</param>
        /// <param name="remotePath">远程文件目录</param>
        /// <param name="remotePath">远程文件名</param>
        public void Put(string localFilePath, string remotePath, string remotePathName)
        {
            try
            {
                var remoteFilePath = Path.Combine(remotePath, remotePathName);
                using (var uplfileStream = new FileStream(localFilePath, FileMode.Open))
                {
                    sftpClient.UploadFile(uplfileStream, remoteFilePath, true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// SFTP下载文件
        /// </summary>
        /// <param name="remoteFilePath"></param>
        /// <param name="localFilePath"></param>
        public void Get(string remoteFilePath, string localFilePath)
        {
            try
            {
                using (var fileStream = new FileStream(localFilePath, FileMode.Create))
                {
                    sftpClient.DownloadFile(remoteFilePath, fileStream);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 移动SFTP文件
        /// </summary>
        /// <param name="localPath"></param>
        /// <param name="remotePath"></param>
        public void Move(string localPath, string remotePath)
        {
            try
            {
                sftpClient.RenameFile(localPath, remotePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除SFTP文件
        /// </summary>
        /// <param name="remoteFilePath"></param>
        public void Delete(string remoteFilePath)
        {
            try
            {
                sftpClient.DeleteFile(remoteFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            sftpClient.Disconnect();
            sftpClient.Dispose();
        }
    }
}
