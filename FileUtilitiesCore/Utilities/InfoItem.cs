namespace FileUtilitiesCore.Utilities
{
    public class FileInfoItem
    {
        public string type;
        public string path;
        public long bytes;
        public DateTime created;
        public DateTime lastAccessTime;
        public DateTime lastWriteTime;
    }

    public class DirectoryInfoItem : FileInfoItem
    {
        public int items;
        public int files;
        public int subdirectories;
    }
}