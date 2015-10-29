using UnityEngine;
using System.Collections;


public abstract class SplashScreenController : SceneController
{
    [SerializeField]
    private float crossFadeDuration = 0.25f;


    protected override void Awake()
    {
        base.Awake();

        visible = false;
        interactable = false;
    }
    
    protected virtual void SetAlpha( float alpha )
    {
        if( canvasGroups != null )
        {
            for( int i = 0; i < canvasGroups.Length; i++ )
            {
                if( canvasGroups[i] )
                    canvasGroups[i].alpha = alpha;
            }
        }
    }
    
    public override void SetVisible( bool value, bool animated, System.Action callback )
    {
        if( value )
        {
            if( animated && (canvasGroups != null) && (canvasGroups.Length > 0) )
            {
                StopAllCoroutines();
                StartCoroutine( FadeInAnimation( callback ) );
            }
            
            else
            {
                SetVisible( true );
                SetAlpha( 1.0f );
                
                if( callback != null )
                    callback();
            }
        }
        
        else
        {
            if( animated && (canvasGroups != null) && (canvasGroups.Length > 0) )
            {
                StopAllCoroutines();
                StartCoroutine( FadeOutAnimation( callback ) );
            }
            
            else
            {
                SetVisible( false );
                
                if( callback != null )
                    callback();
            }
        }
    }
    
    IEnumerator FadeInAnimation( System.Action callback )
    {
        float t = 0.0f;
        float max_t = Mathf.Clamp( crossFadeDuration, 0.001f, 10.0f );
        
        SetVisible( true );

        while( t < max_t )
        {
            t += Time.deltaTime;
        
            SetAlpha( t / max_t );

            yield return null;
        }
        
        SetAlpha( 1.0f );
        
        if( callback != null )
            callback();
    }

    IEnumerator FadeOutAnimation( System.Action callback )
    {
        float t = 0.0f;
        float max_t = Mathf.Clamp( crossFadeDuration, 0.001f, 10.0f );
        
        while( t < max_t )
        {
            t += Time.deltaTime;
        
            SetAlpha( 1.0f - (t / max_t) );

            yield return null;
        }
        
        SetAlpha( 0.0f );
        SetVisible( false );
        
        if( callback != null )
            callback();
    }
}

