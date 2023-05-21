using System;

[Serializable]
public class PosterDataDTO
{
    public string message { set; get; }
    public int posterId { set; get; }
    public PosterDTO poster{ set; get; }

    public PosterDataDTO()
    {
        
    }
}
