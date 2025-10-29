using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_Panel : MonoBehaviour
    {
        private readonly Stack<DebugUI_Group> _groups = new();
        private Image _imageCached;

        internal Image Image {
            get {
                if (_imageCached) return _imageCached;

                _imageCached = GetComponent<Image>();

                return _imageCached;
            }
        }

        private void Add(MonoBehaviour obj) => Add(obj.gameObject);

        private void Add(GameObject obj) {
            if (!_groups.TryPeek(out DebugUI_Group group)) {
                obj.transform.SetParent(transform, false);

                return;
            }

            group.AddChild(obj);
        }

        internal void Label(Func<string> updateText) {
            DebugUI_Label label = Instantiate(LabelPrefab);
            Add(label);
            label.Init(updateText);
        }

        internal void FoldoutGroup(Func<string> updateText, bool defaultShown) {
            DebugUI_FoldoutGroup foldout = Instantiate(FoldoutGroupPrefab);
            Add(foldout);
            foldout.Init(updateText, defaultShown);
            _groups.Push(foldout);
        }

        internal void IfGroup(Func<bool> condition) {
            var group = Instantiate(GroupPrefab).AddComponent<DebugUI_IfGroup>();
            Add(group);
            group.Init(condition);
            _groups.Push(group);
        }

        internal void ElseGroup() {
            DebugUI_Group lastGroup = _groups.Pop();

            if (lastGroup is not DebugUI_IfGroup ifGroup)
                throw new ArgumentException(
                    "[CookieUtils.Debugging] An else group can only be started after an if group!"
                );

            var group = Instantiate(GroupPrefab).AddComponent<DebugUI_ElseGroup>();
            Add(group);
            group.Init(ifGroup);
            _groups.Push(group);
        }

        internal void SwitchGroup<T>(Func<T> condition) {
            var group = Instantiate(GroupPrefab).AddComponent<DebugUI_SwitchGroup<T>>();
            Add(group);
            group.Init(condition);
        }

        internal void CaseGroup<T>(Func<T> value) {
            var group = Instantiate(GroupPrefab).AddComponent<DebugUI_CaseGroup<T>>();
            Add(group);
            group.Init(value);
        }

        internal void EndGroup() {
            _groups.TryPop(out _);
        }

        #region Prefabs

        private DebugUI_FoldoutGroup _foldoutGroupPrefab;
        private DebugUI_Label _labelPrefab;
        private GameObject _groupPrefab;

        private DebugUI_FoldoutGroup FoldoutGroupPrefab {
            get {
                if (!_foldoutGroupPrefab)
                    _foldoutGroupPrefab = Resources.Load<DebugUI_FoldoutGroup>("DebugUI/Prefabs/FoldoutGroup");

                return _foldoutGroupPrefab;
            }
        }

        private DebugUI_Label LabelPrefab {
            get {
                if (!_labelPrefab)
                    _labelPrefab = Resources.Load<DebugUI_Label>("DebugUI/Prefabs/Label");

                return _labelPrefab;
            }
        }

        private GameObject GroupPrefab {
            get {
                if (!_groupPrefab)
                    _groupPrefab = Resources.Load<GameObject>("DebugUI/Prefabs/Group");

                return _groupPrefab;
            }
        }

        #endregion
    }
}