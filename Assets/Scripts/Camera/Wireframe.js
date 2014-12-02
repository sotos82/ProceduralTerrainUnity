// Attach this script to a camera, this will make it render in wireframe
function OnPreRender() {
    GL.wireframe = true;
}
function OnPostRender() {
    GL.wireframe = false;
}