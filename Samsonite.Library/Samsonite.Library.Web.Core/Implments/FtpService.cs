using Samsonite.Library.Data.Entity.Models;
using Samsonite.Library.Utility;
using Samsonite.Library.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Samsonite.Library.Web.Core
{
    public class FtpService : IFtpService
    {
        private appEntities _appDB;
        public FtpService(appEntities appEntities)
        {
            _appDB = appEntities;
        }

        /// <summary>
        /// 获取Ftp信息
        /// </summary>
        /// <param name="ftpIdentify"></param>
        /// <param name="isDelete"></param>
        /// <returns></returns>
        public FtpDto GetFtp(string ftpIdentify, bool isDelete = false)
        {
            FTPInfo fTPInfo = _appDB.FTPInfo.Where(p => p.FTPIdentify == ftpIdentify).SingleOrDefault();
            if (fTPInfo != null)
            {
                return new FtpDto()
                {
                    FtpName = fTPInfo.FTPName,
                    FtpServerIp = fTPInfo.IP,
                    Port = fTPInfo.Port,
                    UserId = fTPInfo.UserName,
                    Password = fTPInfo.Password,
                    RemoteFilePath = fTPInfo.FilePath,
                    IsDeleteOriginalFile = isDelete
                };
            }
            else
            {
                return null;
            }
        }

        #region SFTP
        /// <summary>
        /// 从SFTP上读取特定格式文件
        /// </summary>
        /// <param name="sFTPHelper">FTP对象</param>
        /// <param name="remotePath">FTP文件目录路径</param>
        /// <param name="localPath">本地文件目录路径</param>
        /// <param name="suffix">文件后缀名</param>
        /// <param name="isDelete">是否删除ftp上文件</param>
        /// <returns></returns>
        public FTPResult GetFilesFromSFtp(SFTPHelper sFTPHelper, string remotePath, string localPath, string suffix, bool isDelete = false)
        {
            FTPResult _result = new FTPResult();
            _result.SuccessFile = new List<string>();
            _result.FailFile = new List<string>();
            //检测文件路径是否存在
            if (!Directory.Exists(localPath)) Directory.CreateDirectory(localPath);
            string remoteFilePath = string.Empty;
            string localFilePath = string.Empty;
            //打开ftp连接
            sFTPHelper.Connect();
            var ftpFileNames = sFTPHelper.GetFileList(remoteFilePath, suffix);
            //读取文件
            foreach (var file in ftpFileNames)
            {
                try
                {
                    remoteFilePath = Path.Combine(remoteFilePath, file);
                    localFilePath = Path.Combine(localPath, file);
                    //下载文件到本地
                    sFTPHelper.Get(remoteFilePath, localFilePath);
                    _result.SuccessFile.Add(localFilePath);
                    //删除ftp上的文件
                    if (isDelete)
                    {
                        sFTPHelper.Delete(remoteFilePath);
                    }
                }
                catch
                {
                    _result.FailFile.Add(file);
                }
            }
            return _result;
        }

        /// <summary>
        /// 上传文件到SFTP
        /// </summary>
        /// <param name="sFTPHelper">FTP对象</param>
        /// <param name="localFilePath">本地文件路径</param>
        /// <param name="remotePath">FTP文件目录路径</param>
        /// <returns></returns>
        public FtpPutBatchResult SendXMLToSFtp(SFTPHelper sFTPHelper, string localFilePath, string remotePath)
        {
            try
            {
                //打开ftp连接
                sFTPHelper.Connect();
                //推送文件
                var remoteFileName = Path.GetFileName(localFilePath);
                sFTPHelper.Put(localFilePath, remotePath, remoteFileName);
                return new FtpPutBatchResult()
                {
                    FilePath= localFilePath,
                    Result = true,
                    ResultMessage = string.Empty
                };
            }
            catch (Exception ex)
            {
                return new FtpPutBatchResult()
                {
                    FilePath = localFilePath,
                    Result = false,
                    ResultMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// 批量上传文件到SFTP
        /// </summary>
        /// <param name="sFTPHelper"></param>
        /// <param name="localFilePathList"></param>
        /// <param name="remotePath"></param>
        /// <returns></returns>
        public List<FtpPutBatchResult> SendXMLToSFtp(SFTPHelper sFTPHelper, List<string> localFilePathList, string remotePath)
        {
            List<FtpPutBatchResult> _result = new List<FtpPutBatchResult>();
            if (localFilePathList.Count > 0)
            {
                try
                {
                    //打开ftp连接
                    sFTPHelper.Connect();
                    foreach (string file in localFilePathList)
                    {
                        try
                        {
                            //上传文件
                            var ftpFileName = Path.GetFileName(file);
                            sFTPHelper.Put(file, remotePath, ftpFileName);
                            _result.Add(new FtpPutBatchResult() { FilePath = file, Result = true, ResultMessage = string.Empty });
                        }
                        catch (Exception ex)
                        {
                            _result.Add(new FtpPutBatchResult() { FilePath = file, Result = false, ResultMessage = ex.Message });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return _result;
        }
        #endregion
    }
}
