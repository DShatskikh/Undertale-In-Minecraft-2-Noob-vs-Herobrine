using System.Collections;
using UnityEngine;

public sealed class Bench : Usable
{
    public override IEnumerator AwaitUse()
    {
        Player.Instance.SetFlex(true);
        Player.Instance.enabled = false;
        Player.Instance.GetComponent<Collider2D>().enabled = false;
        Player.Instance.View.GetComponent<Animator>().Play("Sit");
        GetComponent<SpriteRenderer>().sortingOrder = -1;
        
        var target = transform.position + new Vector3(0.06883565f, 0.4410377f, 0);
        
        while (target != Player.Instance.transform.position)
        {
            Player.Instance.transform.position = Vector3.MoveTowards(
                Player.Instance.transform.position, target, 10f * Time.deltaTime);
            yield return null;
        }
        
        yield return new WaitForSeconds(0.2f);
        
        while (true)
        {
            yield return null;

            if (InputManager.Instance.Horizontal != 0 || InputManager.Instance.Vertical != 0)
            {
                if (InputManager.Instance.Horizontal < 0)
                {
                    target = transform.position + new Vector3(-1.52f, 0, 0);
                }
                else if (InputManager.Instance.Horizontal > 0)
                {
                    target = transform.position + new Vector3(1.52f, 0, 0);
                }
                else
                {
                    target = transform.position + new Vector3(0, -0.24f, 0); 
                }

                while (target != Player.Instance.transform.position)
                {
                    Player.Instance.transform.position = Vector3.MoveTowards(
                        Player.Instance.transform.position, target, 10f * Time.deltaTime);
                    yield return null;
                }
                
                GetComponent<SpriteRenderer>().sortingOrder = 0;
                Player.Instance.GetComponent<Collider2D>().enabled = true;
                Player.Instance.View.GetComponent<Animator>().Play("Idle");
                Player.Instance.enabled = true;
                yield break;
            }
        }
    }
}
