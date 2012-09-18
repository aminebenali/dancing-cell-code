using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NvConstructionUIManager : NvCommonUIManager {
	
	private NvConstructionBuildDialog mConstructionDialog = null;
	
	private void initNGUIConstructionBuildDialog()
	{
		GameObject obj = new GameObject("UIDialog");
		mConstructionDialog = obj.AddComponent<NvConstructionBuildDialog>();
		mConstructionDialog.transform.parent = mNGUI2DDialogCameraTr;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;
		obj.transform.localScale = Vector3.one;	
		
	}
	
	/*
	public NvConstructionBuildDialog createConstructionBuild( CameraLayer layer, string dialogName )
	{
		if ( mNGUI2DDialogCameraTr != null ) 
		{
			if (!mUIObjectMap.ContainsKey(dialogName)) 
			{
				initNGUIConstructionBuildDialog();
				mConstructionDialog.name = dialogName;
				mUIObjectMap.Add(dialogName, mConstructionDialog);
				return mConstructionDialog;
			}
			else 
			{
				Debug.Log("====================>    bjy:    Error!  this name had been used! please change");
				return null;
			}
		}
		else {
			return null;
		}
	}*/
	
	//public void setConstructionBuildDialog( string dialogName, NvUIContext context )
	//{/*
		//if (mUIObjectMap.ContainsKey(dialogName))
		//{
			//clear list
			/*if (context.maskChk((uint)NvUIContext.eMask._LIST_CLEAR_ITEMS))
			{
				((NvConstructionBuildDialog)(mUIObjectMap[dialogName])).clearItems();
			}
			//load list items
			if (context.maskChk((uint)NvUIContext.eMask._LIST_LOAD_ITEMS))
			{
				((NvConstructionBuildDialog)(mUIObjectMap[dialogName])).setDragItems( context.itemList );
			}
			//add list item
			if (context.maskChk((uint)NvUIContext.eMask._LIST_ADD_ITEM))
			{
				((NvConstructionBuildDialog)(mUIObjectMap[dialogName])).addItem( context.itemPlus );
			}
			//remove list item
			if (context.maskChk((uint)NvUIContext.eMask._LIST_REMOVE_ITEM))
			{
				((NvConstructionBuildDialog)(mUIObjectMap[dialogName])).removeItem( context.removeItemIndex );
			}			
			//set build button function
			if (context.maskChk((uint)NvUIContext.eMask._BUTTON_FUN))
			{*/
				/*
				foreach( NvUIButton.ButtonFunctionWithParam cb in context.callbacks )
				{
					((NvConstructionBuildDialog)(mUIObjectMap[dialogName])).OnClickEvent += new NvUIButton.ButtonFunctionWithParam(cb);
				}
				//((NvUIButton)(mUIObjectMap[buttonName])).buttonFunWithParam = buttonContext.buttonFunWithParam;
				((NvConstructionBuildDialog)(mUIObjectMap[dialogName])).buttonFunParam = context.buttonFunParam;
				*/
			//}
		//}
		//else
		//{
		//	Debug.Log("====================>    bjy:    Error!  this name doesn't exist");
		//}			
	//}
}
