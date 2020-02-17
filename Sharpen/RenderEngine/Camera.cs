using System;
using OpenTK;

namespace Sharpen.RenderEngine
{

    /// <summary>The <c>Camera</c> entity to move around the world.</summary>
    /// <remarks>
    ///     Camera holds a <see><c>Position</c></see> and an <see><c>Orientation</c></see>
    ///     which can be set as a whole, or individually for each of their components.
    ///     <example>
    ///         camera = new Camera();
    ///         camera.X += 100f;
    ///         camera.Roll -= 45f;
    ///         camera2 = new Camera();
    ///         camera2.Position = new Vector3(0f, 20f, -10f);
    ///         camera2.Orientation = new Vector3(15f, 0f, 0f);
    ///     </example>
    /// </remarks>
    public class Camera
    {
        /// <value>Position of the camera in the X axis in world units.</value>
        public float X;
        /// <value>Position of the camera in the Y axis in world units.</value>
        public float Y;
        /// <value>Position of the camera in the Z axis in world units.</value>
        public float Z;
        /// <value>Rotation of the camera along the X axis in degrees.</value>
        public float Pitch;
        /// <value>Rotation of the camera along the Y axis in degrees.</value>
        public float Yaw;
        /// <value>Rotation of the camera along the Z axis in degrees.</value>
        public float Roll;

        /// <value>Position of the camera</value>
        /// <remarks>
        ///     Position can be accessed either as a whole, or by specific methods.
        ///     See: <see><c>X</c></see>, <see><c>Y</c></see>, <see><c>Z</c></see>
        ///     When accessed as a whole, it uses a Vector3 where the first coordinate
        ///     is X, the second Y and third is Z.
        /// </remarks>
        public Vector3 Position 
        {
            get { return new Vector3(X, Y, Z); } 
            set { X = value.X; Y = value.Y; Z = value.Z; }
        }

        /// <value>Rotation/Orientation of the camera.</value>
        /// <remarks>
        ///     Orientation can be accessed either as a whole, or by specific methods.
        ///     See: <see><c>Pitch</c></see>, <see><c>Yaw</c></see>, <see><c>Roll</c></see>
        ///     When accessed as a whole, it uses a Vector3 where the first coordinate
        ///     is Pitch, the second Yaw and third is Roll.
        /// </remarks>
        public Vector3 Orientation 
        {
            get { return new Vector3(Pitch, Yaw, Roll); } 
            set { Pitch = value.X; Yaw = value.Y; Roll = value.Z; }
        }

        /// <summary>Creates a new <c>Camera</c> at origin of coordinates with no rotation.</summary>
        public Camera()
        {
            Position = new Vector3(0.0f, 0.0f, 0.0f);
            Orientation = new Vector3(0.0f, 0.0f, 0.0f);
        }

        /// <summary>Gets the view matrix for this <c>Camera</c>.</summary>
        /// <remarks>
        ///     The view matrix is built based on the camera position, the target position
        ///     (where the camera is looking at) and the up direction.
        ///     Position of the camera is defined by the <c>Camera</c> itself;
        ///     The target position is along the Z axis, thus, take the position and add 
        ///     to it the Unit Z vector; the up direction is the Y axis unit vector for 
        ///     obvious reasons.
        ///     Finally, we have to apply the orientation of the camera to get everything
        ///     on a single matrix.
        /// </remarks>
        /// <returns>The camera view matrix.</returns>
        public Matrix4 GetViewMatrix()
        {
            //Vector3 target = new Vector3((float)Math.Cos(Roll) * (float)Math.Sin(MathHelper.DegreesToRadians(Yaw)),
            //                             (float)Math.Cos(Roll) * (float)Math.Cos(MathHelper.DegreesToRadians(Pitch)),
            //                             (float)Math.Sin(MathHelper.DegreesToRadians(Roll)));
            //Matrix4 view = Matrix4.LookAt(Position, target, Vector3.UnitY);
            Matrix4 view = Matrix4.LookAt(Position, Position - Vector3.UnitZ, Vector3.UnitY);
            view *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Pitch));
            view *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Yaw));
            view *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Roll));
            return view;
        }

        /// <summary>Binds this <c>Camera</c> to the render system.</summary>
        public void Bind()
        {
            Engine.BasicRenderer().BindCamera(this);
        }
    }

}