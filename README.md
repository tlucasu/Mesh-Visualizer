# Mesh Visualizer
![Unity](https://img.shields.io/badge/Unity-2021.3.4f1-brightgreen)

Interactive 3D Model Visualizer built with the [Unity](https://unity.com) game engine. Mesh Visualizer allows you to switch between different meshes (models) and apply different materials and textures to them in real-time. You can also experiement viewing the model with different lighting environments, camera angles, and post processing effects.

---
## Usage

1. Download the most recent version from [Releases](https://github.com/tlucasu/Mesh-Visualizer/releases)
2. Unzip the application into a directory of your choice
3. Launch the **MeshVisualizer.exe**

The scene will automatically load a default model, material and texture.

### Move, Rotate and Scale the Model

Open the Transform panel by navigating to the `Transform` tab, on the left of the screen using the mouse cursor (or touch on mobile). The Transform panel will have several sliders you can use to adjust the position, rotation and scale of the model. Clicking any of the reset buttons will reset the values back to their defaults.

#### Transform
 - X Axis - Sets the position of the model's position along the x axis
 - Y Axis - Sets the position of the model's position along the y axis
 - Z Axis - Sets the position of the model's position along the z axis

#### Rotation
 - X Axis - Sets the rotation of the model along the x axis
 - Y Axis - Sets the rotation of the model along the y axis
 - Z Axis - Sets the rotation of the model along the z axis

> You may also rotate the model by dragging on an empty space on the screen. The model will rotate in the direction of the drag.

### Scale
 - Size - Increases or decreases the size of the model along the x, y, and z axis (at the same time)


### Switch Model, Texture, or Material

Open the appropriate panel by navigating to the `Model`, `Material`, or `Texture` tab, on the left of the screen using the mouse cursor (or touch on mobile). Click any of the non-highlighted buttons in the panel to switch to that model, material or texture respectively.

Switching out any of the assets will just switch out that particular asset. For example switching materials will switch out the material on the current model to the new material and reapply the textures from the previous material.

### Change Lighting

Open the Lighting panel by navigating to the `Lighting` tab, on the left of the screen using the mouse cursor (or touch on mobile). Here you can change the current skybox between 3 preset skyboxes; Default Skybox, Dark Skybox, and No Skybox. Switching between them will change the background and ambient lighting. You may also toggle several preset lights below.

### Change Camera & Post Processing Effects

Changing the camera or toggling post processing effects is located in the Camera panel. Open the panel by navigating to the `Camera` tab, on the left side of the screen using the mouse cursor (or touch on mobile). At the under 'Cameras' there are three options to switch between. Each option views the model from a different position. Under the 'Post Processing' header you will see several effects you can toggle.

 - Bloom - Makes bright areas on the screen glow
 - Color Adjustments - Shifts the tone, brightness, and contrast of the screen
 - Vignette - Darkens the edges of the screen
 - Color Curves - Adjusts specific ranges in hue, and saturation
 - Lens Distortion - Distorts the screen to appear as if looking through a lens
 - Tonemapping - Remaps the color values on the screen to HDR colours

---
## How to Build
### Download

1. Clone this repository:
    ```bash
    git clone https://github.com/tlucasu/Mesh-Visualizer.git
    ```

2. If you do not already have **Unity 2021.3.x**, you can install it via the [Unity Hub](https://unity.com/download) or [Downloads](https://unity3d.com/get-unity/download/archive) page

3. Open the project in Unity 

4. Once Unity has finished initializing the project, proceed to building the asset files

### Build Asset Files
1. In Unity, open the **Addressables Asset Group** window using the menu `Window > Asset Management > Addressables > Groups`

2. In the **Addressables Asset Group** window navigate to `Build > New Build > Default Build Script`

3. Wait for build to finish then proceed to building the standalone player


### Build Standalone Player
Before building the player, you must build the asset files. If you have not done that yet see [Build Asset Files](#build-asset-files)

1. In Unity open the **Build Settings** window using the menu `File > Build Settings..`

2. Select `Windows, Mac, Linux` under Platform

2. Click the **Build** button, and select an output directory




---
## Adding Assets

1. Import the asset into anywhere under `Mesh-Visualizer/Assets/..`

2. In Unity select the asset in the **Project** window 

3. In the **Inspector**, at the top, mark the asset as Addressable
    
    ![MarkAsAddressable](https://cdn.discordapp.com/attachments/507657297078779906/986779730223202324/inspector-addressable-checked.PNG)

4. (Optional) Change the Address to a readable name

5. Click the **Select** button to open the **Addressables Asset Group** window and highlight the asset

6. Assign the asset with the appropriate label: Model, Texture, or Material

    ![AssignLabel](https://cdn.discordapp.com/attachments/507657297078779906/986779740797038663/assign-addressable-label.png)

New assets will be available immediately when pressing the play button in the editor. They will show up in their respective object panel automatically.

**Note: if building a standalone player you must [rebuild the asset files](#build-asset-files).**