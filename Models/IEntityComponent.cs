using System;
using Nova_Engine.Object;

namespace Nova_Engine.NovaLib.Editor;

public interface IEntityComponent
{
    public IntPtr GetPointer();
    public void LoadComponent(Entity entity);
}