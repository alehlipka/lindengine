using Assimp;
using LindEngine.Core.Exceptions;
using LindEngine.Core.Models.Meshes;

namespace LindEngine.Core.Models;

public class Model : LindenMesh
{
    public readonly string Name;

    internal Scene scene;
    internal Node rootNode = new Node();

    public Model(string filename, bool hasAnimations) : base(hasAnimations)
    {
        PostProcessSteps postProcessSteps = PostProcessSteps.Triangulate
            | PostProcessSteps.FlipUVs
            | PostProcessSteps.CalculateTangentSpace
            | PostProcessSteps.GenerateSmoothNormals;

        LoadModel(filename, postProcessSteps);
    }

    private void LoadModel(string filename, PostProcessSteps postProcessSteps)
    {
        AssimpContext importer = new AssimpContext();
        scene = importer.ImportFile(filename, postProcessSteps);

        if (
            scene == null
            || scene.RootNode == null
            || (scene.SceneFlags & SceneFlags.Incomplete) == SceneFlags.Incomplete
        ) {
            throw new LoadModelException($"Assimp importer error: Scene creation failure for model '{Name}'");
        }

        rootNode = scene.RootNode;
    }
}
