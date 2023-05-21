using System;

[Serializable]
public class PosterDTO
{
    public string posterName{ set; get; } 
    public string fileUrl{ set; get; }
    public int roomId{ set; get; }
    public int userId{ set; get; }
    public int posterId{ set; get; }
    public PositionDTO position{ set; get; }
    
    public PosterDTO(){}
}
