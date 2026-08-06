 using Shared.Characters;
 

 namespace Server.DTO.HTTP;
 public record HandshakeResponseDto
    {
        
        public long Id {get; set;} 
        public long RegionId {get; set;}
        public string Name {get; set;} = "";
        public float PosX {get; set;}
        public float PosY {get; set;}
        public float PosZ {get; set;}
        public long UserId {get; set;}

    // CHARACTER

        public CharacterType Type {get; set;} = CharacterType.Default;
        public int CurrentHp {get; set;} = 220;
        public int CurrentMp {get; set;} = 100;
        public int Lvl {get; set;} = 1;
        public long Silver {get; set;} = 0;

    }