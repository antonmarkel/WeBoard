using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Drawables.Cursors;
using WeBoard.Core.Network.Serializable.Cursors;

namespace WeBoard.Client.Services.Managers
{
    public class RemoteCursorManager
    {
        private static RemoteCursorManager? Instance;
        public RemoteCursorManager()
        {
            _font = new Font("C:/Windows/Fonts/arial.ttf");
        }
        public static RemoteCursorManager GetInstance()
        {
            return Instance ?? (Instance = new());
        }

        private RemoteCursorSerializable? _userCursorSerializable;
        private readonly Dictionary<long, RemoteCursor> _cursors = new();
        private readonly Font _font;

        public void JoinMember(string name, long userId)
        {
            var color = GenerateUserColor(userId);
            _cursors[userId] = new RemoteCursor(name,color,_font);
        }

        public void InitUserCursor(long userId)
        {
            _userCursorSerializable = new RemoteCursorSerializable
            {
                Position = new(),
                UserId = userId
            };
        }

        public void UpdateUserCursor(Vector2f position)
        {
            if (_userCursorSerializable is null)
                return;

            _userCursorSerializable.Position = position;
            NetworkManager.GetInstance().SendCursorUpdate(_userCursorSerializable.Serialize());
        }

        public void UpdateRemoteCursor(string data)
        {
            var serializableCursor = new RemoteCursorSerializable();
            serializableCursor.Deserialize(data);

            if (!_cursors.TryGetValue(serializableCursor.UserId, out var cursor))
            {
                return;
            }

            cursor.Update(serializableCursor.Position);
        }

        public void RemoveCursor(long userId)
        {
            if (_cursors.ContainsKey(userId))
            {
                _cursors.Remove(userId);
            }
        }

        public void Draw(RenderTarget target,RenderStates states)
        {
            foreach (var cursor in _cursors.Values)
            {
                if(cursor.SecondsSinceUpdate < 5)
                    cursor.Draw(target, states);
            }
        }

        private Color GenerateUserColor(long userId)
        {
            var hash = userId.GetHashCode();
            return new Color(
                (byte)(hash & 0xFF),
                (byte)((hash >> 8) & 0xFF),
                (byte)((hash >> 16) & 0xFF)
            );
        }
    }
}

