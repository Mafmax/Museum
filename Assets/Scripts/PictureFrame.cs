using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PictureFrame : MonoBehaviour
{
    public static List<Image> allImages;
    public KeyCode AddImage;
    public KeyCode RemoveImage;
    public Image picture;
    private GameObject center;
    public int FrameNumber = 0;
    private void Awake()
    {
        allImages = new List<Image>();
        AddImage = KeyCode.F;
        RemoveImage = KeyCode.R;
        center = this.transform.GetChild(4).gameObject;
    }
    public void ShowPicture(Image prevPicture)
    {
        if (prevPicture == null)
        {
            center.SetActive(false);
            return;
        }

        var tex = new Texture2D(1024, 1024);
        tex.LoadImage(prevPicture.PictureBytes);

        tex.Apply();
        var renderer = center.GetComponent<Renderer>();
        renderer.material.mainTexture = tex;

        var texRelation = tex.height / (float)tex.width;

        var centerRelation = center.transform.localScale.y / center.transform.localScale.x;

        var multiplier = centerRelation / texRelation;
        float mulX = 1;
        float mulY = 1;
        if (multiplier < 1)
            mulX = multiplier;
        else
            mulY = 1 / multiplier;

        center.transform.localScale = new Vector3(center.transform.localScale.x * mulX, center.transform.localScale.y * mulY, center.transform.localScale.z);
        center.SetActive(true);
    }

    public void RemovePicture()
    {
        if (picture != null)
        {
            picture.FrameNumber = -1;
            Downloader.ChangeImg(picture, -1);
            picture = null;
        }
        ShowPicture(null);
    }
    public Image AddPicture(Image newPicture)
    {
        if (picture != null)
        {
            picture.FrameNumber = -1;
            Downloader.ChangeImg(picture, -1);
        }
        if (newPicture != null)
        {
            gameObject.SetActive(true);
            newPicture.FrameNumber = FrameNumber;
            Downloader.ChangeImg(newPicture, FrameNumber);

        }
        var oldPicture = picture;
        picture = newPicture;
        ShowPicture(picture);
        return oldPicture;
    }

    public string CreateTooltip(bool imgInHand)
    {
        if (imgInHand)
        {
            if (picture != null)
            {

                return $"Поменять [{AddImage}]\nУбрать [{RemoveImage}]";
            }
            else
            {
                return $"Повесить [{AddImage}]";
            }
        }
        else
        {
            if (picture == null)
            {

                return $"Возьмите картину в стенде";
            }
            else
            {
                return $"Снять [{AddImage}]\nУбрать [{RemoveImage}]";
            }
        }

    }

}
