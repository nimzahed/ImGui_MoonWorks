
using MoonWorks;
using MoonWorks.Graphics;
using ImGuiNET;
using ImGuiNET.backends;

namespace YourProject
{

    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            bool isDebug = false;
#if DEBUG
            isDebug = true;
#endif
            
            AppInfo appInfo = new AppInfo("Fig", "FigEditor");
            WindowCreateInfo windowCreateInfo = new WindowCreateInfo("FigEditor", 1280, 720, ScreenMode.Windowed, true, false);
            FramePacingSettings framePacingSettings = FramePacingSettings.CreateCapped(200,200);

            ShaderFormat format = ShaderFormat.SPIRV;

            Game g = new MWGame(appInfo, windowCreateInfo, framePacingSettings, format, isDebug);
            g.Run();
            
        }

    }

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
    }

}
