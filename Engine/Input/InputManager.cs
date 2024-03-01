using Lindengine.Core;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Lindengine.Input;

public static class InputManager
{
    /// <summary>
    /// Gets whether the specified key is released in the current frame but pressed in the previous frame
    /// </summary>
    /// <param name="key">The key to check</param>
    /// <returns>True if the key is released in this frame, but pressed the last frame</returns>
    public static bool IsKeyboardKeyReleased(Keys key)
    {
        return Lind.Engine.Window?.IsKeyReleased(key) ?? false;
    }
        
    /// <summary>
    /// Gets whether the specified key is pressed in the current frame but released in the previous frame
    /// </summary>
    /// <param name="key">The key to check</param>
    /// <returns>True if the key is pressed in this frame, but not the last frame</returns>
    public static bool IsKeyboardKeyPressed(Keys key)
    {
        return Lind.Engine.Window?.IsKeyPressed(key) ?? false;
    }
        
    /// <summary>
    /// Gets a Boolean indicating whether this key is currently down
    /// </summary>
    /// <param name="key">The key to check</param>
    /// <returns>true if key is in the down state; otherwise, false</returns>
    public static bool IsKeyboardKeyDown(Keys key)
    {
        return Lind.Engine.Window?.IsKeyDown(key) ?? false;
    }
        
    /// <summary>
    /// Gets a value indicating whether any key is down
    /// </summary>
    /// <returns>true if any key is down; otherwise, false</returns>
    public static bool IsAnyKeyboardKeyDown()
    {
        return Lind.Engine.Window?.IsAnyKeyDown ?? false;
    }

    public static KeyboardState KeyboardState()
    {
        return Lind.Engine.Window?.KeyboardState
               ?? throw new NullReferenceException("Window is not ready yet");
    }

    public static Vector2 MousePosition()
    {
        return Lind.Engine.Window?.MousePosition ?? Vector2.Zero;
    }
        
    public static bool IsMouseButtonReleased(MouseButton mouseButton)
    {
        return Lind.Engine.Window?.IsMouseButtonReleased(mouseButton) ?? false;
    }
        
    public static bool IsMouseButtonPressed(MouseButton mouseButton)
    {
        return Lind.Engine.Window?.IsMouseButtonPressed(mouseButton) ?? false;
    }
        
    public static bool IsMouseButtonDown(MouseButton mouseButton)
    {
        return Lind.Engine.Window?.IsMouseButtonDown(mouseButton) ?? false;
    }

    public static bool IsAnyMouseButtonDown()
    {
        return Lind.Engine.Window?.IsAnyMouseButtonDown ?? false;
    }

    public static MouseState MouseState()
    {
        return Lind.Engine.Window?.MouseState
               ?? throw new NullReferenceException("Window is not ready yet");
    }

    public static Vector2i MousePositionTopLeft()
    {
        Vector2 mousePosition = Lind.Engine.Window?.MousePosition ?? Vector2.Zero;

        return new Vector2i((int)mousePosition.X, (int)mousePosition.Y);
    }
    
    public static Vector2i MousePositionBottomLeft()
    {
        int windowHeight = Lind.Engine.Window?.ClientSize.Y ?? 0;
        Vector2i mousePosition = MousePositionTopLeft();
        mousePosition.Y = windowHeight - mousePosition.Y;

        return mousePosition;
    }
        
    public static Vector2i PointToScreen(Vector2i point)
    {
        return Lind.Engine.Window?.PointToScreen(point) ?? Vector2i.Zero;
    }
        
    public static Vector2i PointToClient(Vector2i point)
    {
        return Lind.Engine.Window?.PointToClient(point) ?? Vector2i.Zero;
    }

    public static bool IsGamePadKeyReleased(GamePadButton button)
    {
        return Lind.Engine.Window?.JoystickStates[0]?.IsButtonReleased((int)button) ?? false;
    }
        
    public static bool IsGamePadKeyPressed(GamePadButton button)
    {
        return Lind.Engine.Window?.JoystickStates[0]?.IsButtonPressed((int)button) ?? false;
    }
        
    public static bool IsGamePadKeyDown(GamePadButton button)
    {
        return Lind.Engine.Window?.JoystickStates[0]?.IsButtonDown((int)button) ?? false;
    }
        
    public static Hat GetGamePadHatState()
    {
        return Lind.Engine.Window?.JoystickStates[0]?.GetHat(0) ?? Hat.Centered;
    }
        
    public static float GetAxis(GamePadAxis axis)
    {
        return Lind.Engine.Window?.JoystickStates[0]?.GetAxis((int)axis) ?? 0;
    }
}