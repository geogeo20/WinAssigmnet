using System.Collections.Generic;
using UnityEngine;

public class PoolList<T> where T : MonoBehaviour
{
    public PoolList()
    {
        ItemsDisplayed = new List<T>();
        poolItems = new List<T>();
    }

    public List<T> ItemsDisplayed { get; private set; }
    private List<T> poolItems = new List<T>();

    private T itemReference;
    private Transform objectsParent;

    public void Init(T reference, Transform container)
    {
        itemReference = reference;
        objectsParent = container;
    }

    public T CreateItem()
    {
        return GetItem();
    }

    /// <summary>
    /// Adds the item into the recycle pool to use it later on
    /// </summary>
    /// <param name="item"></param>
    public void Delete(T item)
    {
        if (!poolItems.Contains(item))
        {
            if (ItemsDisplayed.Contains(item))
            {
                if (ItemsDisplayed.Contains(item))
                {
                    ItemsDisplayed.Remove(item);
                }

                poolItems.Add(item);
                item.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Adds all the items into the recycle ppol
    /// </summary>
    public void DeleteAll()
    {
        List<T> cachedList = new List<T>(ItemsDisplayed);
        foreach (T item in cachedList)
        {
            Delete(item);
        }
    }

    /// <summary>
    /// Return an item from the pool if there are items available.
    /// Creates one if pool is empty.
    /// </summary>
    /// <returns></returns>
    private T GetItem()
    {
        if (poolItems.Count > 0)
        {
            T returnItem = poolItems[0];
            returnItem.gameObject.SetActive(true);
            returnItem.transform.SetParent(objectsParent);
            returnItem.transform.SetAsFirstSibling();
            returnItem.transform.position = objectsParent.position;
            poolItems.RemoveAt(0);
            ItemsDisplayed.Add(returnItem);
            return returnItem;
        }

        T newObject = GameObject.Instantiate(itemReference, objectsParent);
        newObject.gameObject.SetActive(true);
        newObject.transform.SetAsFirstSibling();
        ItemsDisplayed.Add(newObject);
        return newObject;
    }

    /// <summary>
    /// Add new items to the pool
    /// </summary>
    /// <param name="items"></param>
    public void AddToPool(List<T> items)
    {
        foreach (var item in items)
        {
            if(!poolItems.Contains(item))
            {
                poolItems.Add(item);
                item.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Remove an object from the pool 
    /// </summary>
    /// <param name="item"></param>
    public void RemoveFromPool(T item)
    {
        if (poolItems.Contains(item))
        {
            poolItems.Remove(item);
        }
        else if (ItemsDisplayed.Contains(item))
        {
            ItemsDisplayed.Remove(item);
        }
    }
}