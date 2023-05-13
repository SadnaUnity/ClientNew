using System;

[Serializable]
    public class PosterDTO
    {
        public String posterName{ set; get; }
        public String fileUrl{ set; get; }
        public int roomId{ set; get; }
        public int userId{ set; get; }
        public int posterId{ set; get; }
        public PositionDTO position{ set; get; }
        
        public PosterDTO(){}
    }
