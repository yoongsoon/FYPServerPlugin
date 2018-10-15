using System;


[System.Serializable]
public struct SerializableVector3
{
    /// <summary>
    /// x component
    /// </summary>
    public float x;

    /// <summary>
    /// y component
    /// </summary>
    public float y;

    /// <summary>
    /// z component
    /// </summary>
    public float z;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rX"></param>
    /// <param name="rY"></param>
    /// <param name="rZ"></param>
    public SerializableVector3(float rX, float rY, float rZ)
    {
        x = rX;
        y = rY;
        z = rZ;
    }

    /// <summary>
    /// Returns a string representation of the object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Format("[{0}, {1}, {2}]", x, y, z);
    }

    public static SerializableVector3 operator +(SerializableVector3 mv1, SerializableVector3 mv2)
    {
        return new SerializableVector3(mv1.x + mv2.x, mv1.y + mv2.y, mv1.z + mv2.z);
    }

    public static SerializableVector3 operator -(SerializableVector3 mv1, SerializableVector3 mv2)
    {
        return new SerializableVector3(mv1.x - mv2.x, mv1.y - mv2.y, mv1.z - mv2.z);
    }

    public static SerializableVector3 operator -(SerializableVector3 mv1, float var)
    {
        return new SerializableVector3(mv1.x - var, mv1.y - var, mv1.z - var);
    }

    public static SerializableVector3 operator *(SerializableVector3 mv1, SerializableVector3 mv2)
    {
        return new SerializableVector3(mv1.x * mv2.x, mv1.y * mv2.y, mv1.z * mv2.z);
    }

    public static SerializableVector3 operator *(SerializableVector3 mv, float var)
    {
        return new SerializableVector3(mv.x * var, mv.y * var, mv.z * var);
    }

    public static SerializableVector3 operator %(SerializableVector3 mv1, SerializableVector3 mv2)
    {
        return new SerializableVector3(mv1.y * mv2.z - mv1.z * mv2.y,
                             mv1.z * mv2.x - mv1.x * mv2.z,
                             mv1.x * mv2.y - mv1.y * mv2.x);
    }

    public float this[int key]
    {
        get
        {
            return GetValueByIndex(key);
        }
        set { SetValueByIndex(key, value); }
    }

    private void SetValueByIndex(int key, float value)
    {
        if (key == 0) x = value;
        else if (key == 1) y = value;
        else if (key == 2) z = value;
    }

    private float GetValueByIndex(int key)
    {
        if (key == 0) return x;
        if (key == 1) return y;
        return z;
    }

    public float DotProduct(SerializableVector3 mv)
    {
        return x * mv.x + y * mv.y + z * mv.z;
    }

    public SerializableVector3 ScaleBy(float value)
    {
        return new SerializableVector3(x * value, y * value, z * value);
    }

    public SerializableVector3 ComponentProduct(SerializableVector3 mv)
    {
        return new SerializableVector3(x * mv.x, y * mv.y, z * mv.z);
    }

    public void ComponentProductUpdate(SerializableVector3 mv)
    {
        x *= mv.x;
        y *= mv.y;
        z *= mv.z;
    }

    public SerializableVector3 VectorProduct(SerializableVector3 mv)
    {
        return new SerializableVector3(y * mv.z - z * mv.y,
                             z * mv.x - x * mv.z,
                             x * mv.y - y * mv.x);
    }

    public float ScalarProduct(SerializableVector3 mv)
    {
        return x * mv.x + y * mv.y + z * mv.z;
    }

    public void AddScaledVector(SerializableVector3 mv, float scale)
    {
        x += mv.x * scale;
        y += mv.y * scale;
        z += mv.z * scale;
    }

    public float Magnitude()
    {
        return (float)(Math.Sqrt(x * x + y * y + z * z));
    }

    public float SquareMagnitude()
    {
        return x * x + y * y + z * z;
    }

    public void Trim(float size)
    {
        if (SquareMagnitude() > size * size)
        {
            Normalize();
            x *= size;
            y *= size;
            z *= size;
        }
    }

    public void Normalize()
    {
        float m = Magnitude();
        if (m > 0)
        {
            x = x / m;
            y = y / m;
            z = z / m;
        }
        else
        {
            x = 0;
            y = 0;
            z = 0;
        }
    }


    public SerializableVector3 Normalized()
    {
        float m = Magnitude();
        if (m > 0)
        {
            x = x / m;
            y = y / m;
            z = z / m;
        }
        else
        {
            x = 0;
            y = 0;
            z = 0;
        }
        return this;
    }


    public SerializableVector3 Inverted()
    {
        return new SerializableVector3(-x, -y, -z);
    }

    public SerializableVector3 Unit()
    {
        SerializableVector3 result = this;
        result.Normalize();
        return result;
    }

    public void Clear()
    {
        x = 0;
        y = 0;
        z = 0;
    }

    public static float Distance(SerializableVector3 mv1, SerializableVector3 mv2)
    {
        return (mv1 - mv2).Magnitude();
    }

    public static SerializableVector3 zero()
    {
        return new SerializableVector3(0f, 0f, 0f);
    }

}


