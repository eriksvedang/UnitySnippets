using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// A simple gui container class
/// tries to autoload a texture if a string is passed into the constructor
/// if no texture, container has no width or height
/// position, scale and rotation are implicit local transforms
/// </summary>
public class Container : IList<Container>
{
    Matrix4x4 _matrix = Matrix4x4.identity;

    [SerializeField]
    List<Container> _children = new List<Container>();

    public GUISkin skin = null;
    
    public Container parent;
    public Vector2 position = Vector2.zero;
    public Vector2 scale = Vector2.one;
    public float rotation = 0f;
    public Texture texture = null;
    public float textureWidth { get { return texture == null ? 0f :(float)texture.width * scale.x; } }
    public float textureHeight { get { return texture == null ? 0f: (float)texture.height * scale.y; } }
    public Color colorMultiply = Color.white;

    public Container(Vector2 pPosition) {
        position = pPosition;
    }

    public Container(Vector2 pPosition, string pTexture) {
        position = pPosition;
        texture = (Texture)Resources.Load(pTexture);
    }
    
    public void Update() {
        UpdateElement();
        foreach (var c in _children) {
            c.Update();
        }    
    }

    protected virtual void UpdateElement() { }
    //protected virtual void PreDraw() { }
    //protected virtual void PostDraw() { }
    public void Draw() {
        
        if (this.scale.x == 0f || this.scale.y == 0f) //don't draw things with 0 scale
            return;
        
        Matrix4x4 oldMatrix = GUI.matrix;//we save a copy of the previous matrix here for a recursive style matrix stack
        GUISkin oldSkin = GUI.skin;
        if (skin != null) {
            GUI.skin = skin;
        }
        Color oldColor = GUI.color;
        
        
        GUI.color *= colorMultiply;

        _matrix.SetTRS(position, Quaternion.Euler(0f, 0f, rotation), new Vector3(scale.x, scale.y, 1f)); //create a new matrix
        GUI.matrix *= _matrix; //apply new matrix

      //  PreDraw();
        
        DrawElement(); //draw this
        foreach (var c in this) { //draw children
            if (c != null) {
                c.Draw();
            }
        }

     // PostDraw();
        GUI.skin = oldSkin;
        GUI.color = oldColor;
        GUI.matrix = oldMatrix; //reapply the old matrix
    }
 

    protected virtual void DrawElement() {
        if (texture != null) {
            GUI.DrawTexture(new Rect(0f, 0f, texture.width,texture.height), texture);
        }
    }

    public void MoveToBack() {
        var p = this.parent;
        if (p != null) {
            p.Remove(this);
            p.Insert(0, this);
        }
    }
    public void MoveToFront() {
        var p = this.parent;
        if (p != null) {
            p.Remove(this);
            p.Add(this);
        }
    }
    #region IList
    public IEnumerator<Container> GetEnumerator() {
        return _children.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
        return _children.GetEnumerator();
    }

    public int IndexOf(Container item) {
        return _children.IndexOf(item);
    }

    public void Insert(int index, Container item) {
        item.parent = this;
        _children.Insert(index, item);
    }

    public void RemoveAt(int index) {
        throw new System.NotImplementedException();
    }

    public Container this[int index] {
        get {
            return _children[index];
        }
        set {
            throw new System.NotImplementedException();
        }
    }

    public void Clear() {
        foreach (var c in _children) { 
            c.parent = null;
            c.Clear();
        };
        _children.Clear();
    }

    public bool Contains(Container item) {
        throw new System.NotImplementedException();
    }

    public void CopyTo(Container[] array, int arrayIndex) {
        throw new System.NotImplementedException();
    }

    public int Count {
        get { throw new System.NotImplementedException(); }
    }

    public bool IsReadOnly {
        get { throw new System.NotImplementedException(); }
    }

    public void Add(Container pChild) {
        pChild.parent = this;
        _children.Add(pChild);
    }
    public bool Remove(Container pChild) {
        pChild.parent = null;
        return _children.Remove(pChild);
    }
    #endregion
}