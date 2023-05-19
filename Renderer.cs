using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

//include GLM library
using GlmNet;


using System.IO;
using System.Diagnostics;

namespace Graphics
{
    class Renderer
    {
        Shader sh;
        
        uint triangleBufferID;
        uint xyzAxesBufferID;

        //3D Drawing
        mat4 ModelMatrix;
        mat4 ViewMatrix;
        mat4 ProjectionMatrix;
        
        int ShaderModelMatrixID;
        int ShaderViewMatrixID;
        int ShaderProjectionMatrixID;

        const float rotationSpeed = 1f;
        float rotationAngle = 0;

        public float translationX=0, 
                     translationY=0, 
                     translationZ=0;

        Stopwatch timer = Stopwatch.StartNew();

        vec3 triangleCenter;

        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");
            Gl.glClearColor(0, 0, 0.4f, 1);
            
            float[] HomeVertices= { 
		      
		        //base             
                //x
                0.0f, 0.0f, 0.0f,1.0f, 0.0f, 0.0f, //R             
                6.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, //R
                6.0f, 0.0f, -5.0f, 1.0f, 0.0f, 1.0f,  //B
                0.0f, 0.0f, -5.0f, 0.0f, 1.0f, 0.0f, //G

                //y
                0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, //R
                0.0f, 0.0f, -5.0f, 0.0f, 1.0f, 0.0f, //G
                0.0f, 5.0f, -5.0f,1.0f, 0.0f, 1.0f, //B
                0.0f, 5.0f, 0.0f, 1.0f, 0.0f, 0.0f, //R

                //z2
                0.0f, 0.0f, -5.0f, 1.0f, 0.0f, 0.0f, //R
                6.0f, 0.0f, -5.0f, 0.0f, 1.0f, 1.0f, //G
                6.0f, 5.0f, -5.0f,1.0f, 0.0f, 0.0f, //R
                0.0f, 5.0f, -5.0f,0.0f, 1.0f, 1.0f, //G

               //y2
                6.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, //R
                6.0f, 0.0f, -5.0f,0.0f, 1.0f, 1.0f, //G
                6.0f, 5.0f, -5.0f, 1.0f, 0.0f, 1.0f, //B
                6.0f, 5.0f, 0.0f,1.0f, 0.0f, 0.0f, //R

               //x2
                0.0f, 5.0f, 0.0f, 1.0f, 0.0f, 0.0f, //R
                6.0f, 5.0f, 0.0f, 0.0f, 1.0f, 1.0f, //G
                6.0f, 5.0f, -5.0f, 1.0f, 0.0f, 0.0f, //R
                0.0f, 5.0f, -5.0f,0.0f, 1.0f, 1.0f, //G

               //z
                0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, //R
                6.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, //G
                6.0f, 5.0f, 0.0f,1.0f, 0.0f, 0.0f, //R
                0.0f, 5.0f, 0.0f,1.0f, 0.0f, 1.0f,  //B

                //up
                3.0f, 9.0f, -2.5f, 0.0f, 0.0f, 0.0f, //p
                0.0f, 5.0f, 0.0f, 0.5f, 1.0f, 1.0f, //G
                0.0f, 5.0f, -5.0f,0.5f, 1.0f, 1.0f, //R
                6.0f, 5.0f, -5.0f,0.5f, 1.0f, 1.0f, //R
                6.0f, 5.0f, 0.0f,0.5f, 1.0f, 1.0f,  //B
                0.0f, 5.0f, 0.0f,0.5f, 1.0f, 1.0f,//G

               //door            
                3.5f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, //G
                2.5f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, //R
                3.5f, 2.5f, 0.0f, 0.0f, 0.0f, 0.0f, //R
                2.5f, 2.5f, 0.0f, 0.5f, 0.0f, 1.0f,

               //windoww1         
                0.5f, 4.0f, 0.0f,0.0f, 0.0f, 0.0f,
                1.5f, 4.0f, 0.0f, 0.0f, 0.0f, 0.0f, //G
                1.5f, 3.0f, 0.0f,0.0f, 0.0f, 0.0f, //R
                0.5f, 3.0f, 0.0f, 0.0f, 0.0f, 0.0f, //G

                //windoww2           
                4.5f, 4.0f, 0.0f,0.0f, 0.0f, 0.0f,
                5.5f, 4.0f, 0.0f,0.0f, 0.0f, 0.0f, //G
                5.5f, 3.0f, 0.0f, 0.0f, 0.0f, 0.0f, //R
                4.5f, 3.0f, 0.0f, 0.0f, 0.0f, 0.0f, //G

            }; // Triangle Center = (10, 7, -5)
            
            triangleCenter = new vec3(3, 10, -3);

            float[] xyzAxesVertices = {
		        //x
		        0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, //R
		        100.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, //R
		        //y
	            0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, //G
		        0.0f, 100.0f, 0.0f, 0.0f, 1.0f, 0.0f, //G
		        //z
	            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f,  //B
		        0.0f, 0.0f, 100.0f, 0.0f, 0.0f, 1.0f,  //B
            };


            triangleBufferID = GPU.GenerateBuffer(HomeVertices);
            xyzAxesBufferID = GPU.GenerateBuffer(xyzAxesVertices);

            // View matrix 
            ViewMatrix = glm.lookAt(
                        new vec3(15, 5, 15), // Camera is at (0,5,5), in World Space
                        new vec3(0, 0, 0), // and looks at the origin
                        new vec3(0, 1, 0)  // Head is up (set to 0,-1,0 to look upside-down)
                );
            // Model Matrix Initialization
            ModelMatrix = new mat4(1);

            //ProjectionMatrix = glm.perspective(FOV, Width / Height, Near, Far);
            ProjectionMatrix = glm.perspective(45.0f, 4.0f / 3.0f, 0.1f, 100.0f);
            
            // Our MVP matrix which is a multiplication of our 3 matrices 
            sh.UseShader();


            //Get a handle for our "MVP" uniform (the holder we created in the vertex shader)
            ShaderModelMatrixID = Gl.glGetUniformLocation(sh.ID, "modelMatrix");
            ShaderViewMatrixID = Gl.glGetUniformLocation(sh.ID, "viewMatrix");
            ShaderProjectionMatrixID = Gl.glGetUniformLocation(sh.ID, "projectionMatrix");

            Gl.glUniformMatrix4fv(ShaderViewMatrixID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            Gl.glUniformMatrix4fv(ShaderProjectionMatrixID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());

            timer.Start();
        }

        public void Draw()
        {
            sh.UseShader();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            #region XYZ axis
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, xyzAxesBufferID);

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array()); // Identity

            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));

            //GL_LINES 1
            Gl.glDrawArrays(Gl.GL_LINES, 0, 6);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);

            #endregion

            #region Animated home
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, triangleBufferID);

            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix.to_array());

            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));

            //GL_POLYGON 2
            Gl.glDrawArrays(Gl.GL_POLYGON, 0, 4);
            Gl.glDrawArrays(Gl.GL_POLYGON, 4, 4);
            Gl.glDrawArrays(Gl.GL_POLYGON, 8, 4);
            Gl.glDrawArrays(Gl.GL_POLYGON, 12, 4);
            Gl.glDrawArrays(Gl.GL_POLYGON, 16, 4);
            Gl.glDrawArrays(Gl.GL_POLYGON, 20, 4);

            //GL_TRIANGLE_FAN  3
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 24, 6);

            //GL_QUAD_STRIP  4
            Gl.glDrawArrays(Gl.GL_QUAD_STRIP, 30, 4);

            //GL_LINE_LOOP  5
            Gl.glDrawArrays(Gl.GL_LINE_LOOP, 34, 4);
            Gl.glDrawArrays(Gl.GL_LINE_LOOP, 38, 4);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
            #endregion
        }
        
        public void Update()
        {
            timer.Stop();
            var deltaTime = timer.ElapsedMilliseconds/1000.0f;
            rotationAngle += deltaTime * rotationSpeed;

            List<mat4> transformations = new List<mat4>();
            transformations.Add(glm.translate(new mat4(1), -1 * triangleCenter));
            transformations.Add(glm.rotate(rotationAngle, new vec3(0, 1, 0)));
            transformations.Add(glm.translate(new mat4(1),  triangleCenter));
            transformations.Add(glm.translate(new mat4(1), new vec3(translationX, translationY, translationZ)));

            ModelMatrix =  MathHelper.MultiplyMatrices(transformations);
            
            timer.Reset();
            timer.Start();
        }
        
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
