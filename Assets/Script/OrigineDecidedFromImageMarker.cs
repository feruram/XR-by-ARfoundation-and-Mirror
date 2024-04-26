using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Unity.XR.CoreUtils;
using UnityEngine.XR.ARFoundation.Samples;
/// <summary>
/// �摜�}�[�J�[���猴�_���߂�
/// </summary>
public class OriginDecideFromImageMaker : MonoBehaviour
{
    /// <summary>
    /// ARTrackedImageManager
    /// </summary>
    [SerializeField] private ARTrackedImageManager _imageManager;

    /// <summary>
    /// ARSessionOrigin
    /// </summary>
    [SerializeField] private XROrigin _xrOrigin;

    /// <summary>
    /// ���[���h�̌��_�Ƃ��ĐU�镑���I�u�W�F�N�g
    /// </summary>
    private GameObject _worldOrigin;

    /// <summary>
    /// �R���[�`��
    /// </summary>
    private Coroutine _coroutine;

    private void OnEnable()
    {
        _worldOrigin = new GameObject("Origin");
        _imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        _imageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    /// <summary>
    /// ���_���߂�
    /// ����͉摜�}�[�J�[�̈ʒu�����_�ƂȂ�
    /// </summary>
    /// <param name="trackedImage">�F�������摜�}�[�J�[</param>
    /// <param name="trackInterval">�F���̃C���^�[�o��</param>
    /// <returns></returns>
    private IEnumerator OriginDecide(ARTrackedImage trackedImage, float trackInterval)
    {
        yield return new WaitForSeconds(trackInterval);
        var trackedImageTransform = trackedImage.transform;
        _worldOrigin.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        _xrOrigin.MakeContentAppearAt(_worldOrigin.transform, trackedImageTransform.position, trackedImageTransform.localRotation);
        _coroutine = null;
    }

    /// <summary>
    /// ���[���h���W��C�ӂ̓_���猩�����[�J�����W�ɕϊ�
    /// </summary>
    /// <param name="world">���[���h���W</param>
    /// <returns></returns>
    public Vector3 WorldToOriginLocal(Vector3 world)
    {
        return _worldOrigin.transform.InverseTransformDirection(world);
    }

    /// <summary>
    /// TrackedImagesChanged���̏���
    /// </summary>
    /// <param name="eventArgs">���o�C�x���g�Ɋւ������</param>
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            StartCoroutine(OriginDecide(trackedImage, 0));
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            if (_coroutine == null) _coroutine = StartCoroutine(OriginDecide(trackedImage, 5));
        }
    }
}
