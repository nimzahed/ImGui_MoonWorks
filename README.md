# ImGui_MoonWorks
This is a [MoonWorks](https://github.com/MoonsideGames/MoonWorks) backend for the immediate mode GUI library, Dear ImGui (https://github.com/ocornut/imgui). 

This backend is built on top of [ImGui.NET](https://github.com/ImGuiNET/ImGui.NET), which allows you to build graphical interfaces using a simple immediate-mode style. ImGui.NET is a .NET Standard library and is compatible with all major .NET runtimes and operating systems.

# Usage
You can view the full script [here](https://github.com/nimzahed/ImGui_MoonWorks/blob/main/example/ImGui.net_MoonWorks.cs).

Alternatively, here’s a quick preview:
### **How This Works**
```csharp

    public class MWGame : Game
    {
        ImGuiMWBackend backend;

        public MWGame(AppInfo appInfo, WindowCreateInfo windowCreateInfo, FramePacingSettings framePacingSettings, ShaderFormat availableShaderFormats, bool debugMode = false) : base(appInfo, windowCreateInfo, framePacingSettings, availableShaderFormats, debugMode)
        {

            backend = new ImGuiMWBackend(this);
        }

        protected override void Draw(double alpha)
        {
            ImGui.Render();

            var io = ImGui.GetIO();
            var drawDataPtr = ImGui.GetDrawData();



            var commandBuffer = GraphicsDevice.AcquireCommandBuffer();
            var swapchainTexture = commandBuffer.AcquireSwapchainTexture(MainWindow);

            if (swapchainTexture != null)
            {
                backend.UploadBuffers(commandBuffer);
                var renderPass = commandBuffer.BeginRenderPass(
                    new ColorTargetInfo(swapchainTexture, Color.Cyan)
                );
                backend.Render(commandBuffer, renderPass);
                commandBuffer.EndRenderPass(renderPass);
            }

            GraphicsDevice.Submit(commandBuffer);
        }

        protected override void Update(TimeSpan delta)
        {
            backend.NewFrame(delta);
            ImGui.ShowDemoWindow();
            backend.EndFrame();
        }
```

# See Also
https://github.com/ocornut/imgui
> Dear ImGui is a bloat-free graphical user interface library for C++. It outputs optimized vertex buffers that you can render anytime in your 3D-pipeline enabled application. It is fast, portable, renderer agnostic and self-contained (no external dependencies).

> Dear ImGui is designed to enable fast iterations and to empower programmers to create content creation tools and visualization / debug tools (as opposed to UI for the average end-user). It favors simplicity and productivity toward this goal, and lacks certain features normally found in more high-level libraries.

> Dear ImGui is particularly suited to integration in games engine (for tooling), real-time 3D applications, fullscreen applications, embedded applications, or any applications on consoles platforms where operating system features are non-standard.


https://github.com/cimgui/cimgui
> This is a thin c-api wrapper for the excellent C++ intermediate gui imgui. This library is intended as a intermediate layer to be able to use imgui from other languages that can interface with C .

https://github.com/ImGuiNET/ImGui.NET
> This is a .NET wrapper for the immediate mode GUI library. ImGui.NET lets you build graphical interfaces using a simple immediate-mode style. ImGui.NET is a .NET Standard library, and can be used on all major .NET runtimes and operating systems.
