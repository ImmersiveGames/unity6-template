﻿namespace _ImmersiveGames.Scripts.Utils.SerializedSystems {
    public interface ISerializer {
        string Serialize<T>(T obj);
        T Deserialize<T>(string json);
    }
}