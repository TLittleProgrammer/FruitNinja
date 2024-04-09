using Cysharp.Threading.Tasks;
using Runtime.UI.Screens;
using UnityEngine;

namespace Runtime.Infrastructure.Factories
{
    public interface IUIFactory
    {
        TResult LoadScreen<TResult>(ScreenType screenType, Transform parent) where TResult : Object;
        UniTask<TResult> LoadUIObjectByPath<TResult>(string path, Transform parent) where TResult : Object;
    }
}