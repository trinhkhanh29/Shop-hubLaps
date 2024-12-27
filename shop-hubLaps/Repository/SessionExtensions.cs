// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System;

namespace shop_hubLaps.Extensions
{
    public static class SessionExtensions
    {
        // Lưu đối tượng vào session sau khi serialize
        public static void SetObject(this ISession session, string key, object value)
        {
            var serializedObject = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // Bỏ qua vòng lặp tham chiếu
            });
            session.SetString(key, serializedObject);
        }

        // Lấy đối tượng từ session sau khi deserialized
        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
