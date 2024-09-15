using System;

namespace Nova_Engine.NovaLib.Editor;

public interface IEntityComponent
{
    public IntPtr GetPointer();
    public void LoadComponent();
}