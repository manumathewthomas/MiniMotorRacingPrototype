using UnityEngine;
using System.Collections;

public class AutoTransparent : MonoBehaviour
{
    private Shader m_OldShader = null;
    private Color m_OldColor = Color.black;
    private float m_Transparency = 0.3f;
    private const float m_TargetTransparancy = 0.3f;
    private const float m_FallOff = 0.1f; // returns to 100% in 0.1 sec

    public void BeTransparent()
    {
        // reset the transparency;
        m_Transparency = m_TargetTransparancy;
        if (m_OldShader == null)
        {
            // Save the current shader
            m_OldShader = renderer.material.shader;
            m_OldColor  = renderer.material.color;
			foreach(Material m in this.renderer.materials)
           		m.shader = Shader.Find("Transparent/Diffuse");
        }
    }
    void Update()
    {
        if (m_Transparency < 1.0f)
        {
			foreach(Material m in this.renderer.materials)
			{
			//Debug.Log("Materialname:"+m.name);
            Color C = m.color;
            C.a = m_Transparency;
            m.color = C;
			}
        }
        else
        {
            // Reset the shader
			
			foreach(Material m in this.renderer.materials)
			{
            m.shader = m_OldShader;
            m.color = m_OldColor;
			}
            // And remove this script
            Destroy(this);
        }
        m_Transparency += ((1.0f-m_TargetTransparancy)*Time.deltaTime) / m_FallOff;
    }
}