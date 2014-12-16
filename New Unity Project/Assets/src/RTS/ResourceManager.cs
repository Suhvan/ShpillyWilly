﻿using UnityEngine;
using System.Collections;

namespace RTS
{
    public static class ResourceManager
    {
        public static float ScrollSpeed { get { return 5; } }
        public static float ZoomSpeed { get { return 1; } }
        public static int ScrollWidth { get { return 15; } }
        public static int BuildSpeed { get { return 2; } }

        private static GameObjectList gameObjectList;
        public static void SetGameObjectList(GameObjectList objectList)
        {
            gameObjectList = objectList;
        }

        public static GameObject GetBuilding(string name)
        {
            return gameObjectList.GetBuilding(name);
        }

        public static GameObject GetUnit(string name)
        {
            return gameObjectList.GetUnit(name);
        }

        public static GameObject GetWorldObject(string name)
        {
            return gameObjectList.GetWorldObject(name);
        }

        public static GameObject GetPlayerObject()
        {
            return gameObjectList.GetPlayerObject();
        }

        public static Texture2D GetBuildImage(string name)
        {
            return gameObjectList.GetBuildImage(name);
        }
    }
}