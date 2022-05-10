namespace Mapbox.Examples
{
	using Mapbox.Unity.Map;
	using Mapbox.Unity.Utilities;
	using Mapbox.Utils;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using System;
    using UnityEngine.XR.Interaction.Toolkit.Inputs;
    using UnityEngine.InputSystem;
    using UnityEngine.UI;
    using System.Collections.Generic;

    public class QuadTreeCameraMovement : MonoBehaviour
	{
		public InputActionReference moveXAction = null;
		public InputActionReference moveYAction = null;
		public InputActionReference zoomAction = null;
		public InputActionReference markerAction = null;
		public InputActionReference distanceAction = null;
		public InputActionReference showMapAction = null; 

		public GameObject map;

		public Material[] skybox;
		private Material currentSkybox;

		public Text levelPoints;
		string levelMsg;
		string errorMsg;

		[SerializeField]
		[Range(1, 20)]
		public float _panSpeed = 0.7f;

		[SerializeField]
		public float _zoomSpeed = 0.25f;

		[SerializeField]
		public Camera _referenceCamera;

		[SerializeField]
		public AbstractMap _mapManager;

		[SerializeField]
		bool _useDegreeMethod;

		[SerializeField]
		float _spawnScale = 100f;

		[SerializeField]
		GameObject _markerPrefab;
		
		[SerializeField]
		GameObject _targetPrefab;

		Vector2d location0;
		Vector2d location1;
		Vector2d location2;
		Vector2d location3;
		Vector2d location4;

		double result;
		double total;
		int locationCounter;
		int firstPass;
		int notAllowed;

		Vector2d markerLatLong;
		Vector2d latitudeLongitude;

		List<Vector2d> locations;

		private Vector2 zoomAxis;
		private Vector2 thumbAxis;
		private Vector3 _origin;
		private Vector3 _mousePosition;
		private Vector3 _mousePositionPrevious;
		private bool _shouldDrag;
		private bool _isInitialized = false;
		private Plane _groundPlane = new Plane(Vector3.up, 0);
		private bool _dragStartedOnUI = false;

		void Awake()
		{
			if (null == _referenceCamera)
			{
				_referenceCamera = GetComponent<Camera>();
				if (null == _referenceCamera) { Debug.LogErrorFormat("{0}: reference camera not set", this.GetType().Name); }
			}
			_mapManager.OnInitialized += () =>
			{
				_isInitialized = true;
			};
            moveXAction.action.performed += MoveX;
            moveYAction.action.performed += MoveY;
			zoomAction.action.performed += ZoomThumbstick;
			markerAction.action.performed += CreateMarker;
			distanceAction.action.performed += CalculateDistance;
			showMapAction.action.performed += ShowMap;

			location0 = new Vector2d(37.8097997, -122.4105405); //pier39
			location1 = new Vector2d(40.7821132, -73.9702457); //central park
			location2 = new Vector2d(37.8632798, -122.5878597); //muir beach
			location3 = new Vector2d(25.7768336, -80.1843875); //miami
			location4 = new Vector2d(40.7579174, -73.9861626); //times square
			locations = new List<Vector2d> { location0, location1, location2, location3, location4 };

			locationCounter = 0;
			total = 0;
			firstPass = 0;
			notAllowed = 0;

			levelMsg = "";
			errorMsg = "";

			currentSkybox = skybox[1];
			
		}

		public void Update()
		{
			if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject())
			{
				_dragStartedOnUI = true;
			}

			if (Input.GetMouseButtonUp(0))
			{
				_dragStartedOnUI = false;
			}
		}


		private void LateUpdate()
		{
			if (!_isInitialized) { return; }

			if (!_dragStartedOnUI)
			{
				if (Input.touchSupported && Input.touchCount > 0)
				{
					HandleTouch();
				}
				else
				{
					HandleMouseAndKeyBoard();
					HandleThumbstick();

					FixMarker();
                }
            }
		}

		private void ShowMap(InputAction.CallbackContext ctx)
		{
			var value = ctx.ReadValue<float>();
			if (notAllowed == 0)
            {
				if (value > 0)
				{
					if (map.activeInHierarchy)
					{
						map.SetActive(false);
					}
					else
					{
						map.SetActive(true);
					}
				}
			}
		}

		void MoveX(InputAction.CallbackContext context)
        {
			if (notAllowed == 0)
            {
				thumbAxis.x = context.ReadValue<float>();
				//Debug.Log("X VALUE: " + thumbAxis.x);
			}
		}

        void MoveY(InputAction.CallbackContext context)
        {
			if (notAllowed == 0)
			{
				thumbAxis.y = context.ReadValue<float>();
				//Debug.Log("Y VALUE: " + thumbAxis.y);
			}
		}

		void ZoomThumbstick(InputAction.CallbackContext ctx)
        {
			if (notAllowed == 0)
            {
				zoomAxis.y = ctx.ReadValue<float>();
            }
			
        }

		public void HandleThumbstick()
        {
			var zoom = Mathf.Max(0.0f, Mathf.Min(_mapManager.Zoom + zoomAxis.y * _zoomSpeed, 21.0f));
			//Debug.Log("ZOOM:" + zoom);
			if (Math.Abs(zoom - _mapManager.Zoom) > 0.0f)
			{
				_mapManager.UpdateMap(_mapManager.CenterLatitudeLongitude, zoom);
			}

			if (Math.Abs(thumbAxis.x) > 0.0f || Math.Abs(thumbAxis.y) > 0.0f)
			{
				// Get the number of degrees in a tile at the current zoom level.
				// Divide it by the tile width in pixels ( 256 in our case)
				// to get degrees represented by each pixel.
				float factor = _panSpeed * (Conversions.GetTileScaleInDegrees((float)_mapManager.CenterLatitudeLongitude.x, _mapManager.AbsoluteZoom));

				latitudeLongitude = new Vector2d(_mapManager.CenterLatitudeLongitude.x + thumbAxis.y * factor * 2.0f, _mapManager.CenterLatitudeLongitude.y + thumbAxis.x * factor * 4.0f);

				//Debug.Log("LAT LONG: " + latitudeLongitude);
				_mapManager.UpdateMap(latitudeLongitude, _mapManager.Zoom);
			}
		}

		void CreateMarker(InputAction.CallbackContext ctx)
        {
			var value = ctx.ReadValue<float>();
			if (notAllowed == 0)
            {
				var toRemove = GameObject.Find("CustomMarkerPrefab(Clone)");
				if (toRemove != null)
				{
					Destroy(toRemove);
				}

				//Debug.Log("primary button value: " + value);
				if (value > 0 && map.activeInHierarchy)
				{
					var instance = Instantiate(_markerPrefab);

					instance.transform.localPosition = _mapManager.GeoToWorldPosition(latitudeLongitude, true);
					instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
					markerLatLong = new Vector2d(latitudeLongitude.x, latitudeLongitude.y);

					instance.transform.parent = _mapManager.transform;
				}
			}
        }

		void FixMarker()
        {
			try 
            {
				var marker = GameObject.Find("CustomMarkerPrefab(Clone)");
				marker.transform.position = _mapManager.GeoToWorldPosition(markerLatLong, true);

				var targetMarker = GameObject.Find("TargetMarkerPrefab(Clone)");
				var targetMarkerLocation = new Vector2d(locations[locationCounter].x, locations[locationCounter].y);
				targetMarker.transform.position = _mapManager.GeoToWorldPosition(targetMarkerLocation, true);
			}
			catch (Exception ex)
            {
				Debug.Log("No marker available");
            }
			
		}

		static double toRadians(
		   double angleIn10thofaDegree)
		{
			// Angle in 10th
			// of a degree
			return (angleIn10thofaDegree *
						   Math.PI) / 180;
		}
		static double distance(double lat1,
							   double lat2,
							   double lon1,
							   double lon2)
		{

			// The math module contains
			// a function named toRadians
			// which converts from degrees
			// to radians.
			lon1 = toRadians(lon1);
			lon2 = toRadians(lon2);
			lat1 = toRadians(lat1);
			lat2 = toRadians(lat2);

			// Haversine formula
			double dlon = lon2 - lon1;
			double dlat = lat2 - lat1;
			double a = Math.Pow(Math.Sin(dlat / 2), 2) +
					   Math.Cos(lat1) * Math.Cos(lat2) *
					   Math.Pow(Math.Sin(dlon / 2), 2);

			double c = 2 * Math.Asin(Math.Sqrt(a));

			// Radius of earth in
			// kilometers. Use 3956
			// for miles
			double r = 6371;

			// calculate the result
			return (c * r);
		}

		void CalculateDistance(InputAction.CallbackContext ctx)
        {
			var value = ctx.ReadValue<float>();
			if (value > 0 && locationCounter < 5)
			{
				var toRemove = GameObject.Find("CustomMarkerPrefab(Clone)");
				var warningColor = new Color(0.5943396f, 0.03644537f, 0.03644537f, 1);
				if (!map.activeInHierarchy)
                {
					errorMsg = "Map disabled. Interactions unavailable.";
					//print(errorMsg);
					levelPoints.color = warningColor;
					levelPoints.text = errorMsg;
				}
				else if (toRemove == null)
				{
					errorMsg = "Please place a destination marker.";
					//print(errorMsg);
					levelPoints.color = warningColor;
					levelPoints.text = errorMsg;
				}
				else if (toRemove != null)
                {
					var destination = locations[locationCounter];
					result = distance(destination.x, markerLatLong.x, destination.y, markerLatLong.y);
					total += result;

					if (firstPass == 0)
                    {
						firstPass = 1;
						notAllowed = 1;

						ScoreOutput(result);

						var instance = Instantiate(_targetPrefab);
						var instanceLatLong = new Vector2d(destination.x, destination.y);
						instance.transform.localPosition = _mapManager.GeoToWorldPosition(instanceLatLong, true);
						instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);

						instance.transform.parent = _mapManager.transform;
					}
					else if (firstPass == 1)
                    {
						var targetToRemove = GameObject.Find("TargetMarkerPrefab(Clone)");
						Destroy(targetToRemove);

						locationCounter++;

						if (locationCounter < 5)
                        {
							firstPass = 0;
							notAllowed = 0;
						}

						ChangeBackground();
                    }
				}
				if (locationCounter == 4 && firstPass == 1)
				{
					string breaker = "_______________________\n";
					levelMsg += breaker + String.Format("Total: {0} km", Math.Round(total, 4));
					levelPoints.text = levelMsg;
				}
			}
		}

		void ScoreOutput(double levelScore)
        {
			levelMsg += String.Format("> Level {0}: {1} km\n", locationCounter+1, Math.Round(levelScore, 4));
			//Debug.Log(levelMsg);
			levelPoints.color = Color.blue;
			levelPoints.text = levelMsg;
		}

		void ChangeBackground()
        {
			RenderSettings.skybox = currentSkybox;
			DynamicGI.UpdateEnvironment();

			for (int i = 0; i < skybox.Length; i++)
			{
				if (i == skybox.Length - 1)
				{
					break;
				}
				if (currentSkybox == skybox[i])
				{
					currentSkybox = skybox[i + 1];
					break;
				}
			}
		}

		void HandleMouseAndKeyBoard()
		{
			// zoom
			float scrollDelta = 0.0f;
			scrollDelta = Input.GetAxis("Mouse ScrollWheel");
			//Debug.Log("SCROLL FLOAT: " + scrollDelta);
			ZoomMapUsingTouchOrMouse(scrollDelta);


            //pan keyboard
            float xMove = Input.GetAxis("Horizontal");
            float zMove = Input.GetAxis("Vertical");

            PanMapUsingKeyBoard(xMove, zMove);

			//pan controller

            //pan mouse
            PanMapUsingTouchOrMouse();
		}

		void HandleTouch()
		{
			float zoomFactor = 0.0f;
			//pinch to zoom.
			switch (Input.touchCount)
			{
				case 1:
					{
						PanMapUsingTouchOrMouse();
					}
					break;
				case 2:
					{
						// Store both touches.
						Touch touchZero = Input.GetTouch(0);
						Touch touchOne = Input.GetTouch(1);

						// Find the position in the previous frame of each touch.
						Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
						Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

						// Find the magnitude of the vector (the distance) between the touches in each frame.
						float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
						float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

						// Find the difference in the distances between each frame.
						zoomFactor = 0.01f * (touchDeltaMag - prevTouchDeltaMag);
					}
					ZoomMapUsingTouchOrMouse(zoomFactor);
					break;
				default:
					break;
			}
		}

		void ZoomMapUsingTouchOrMouse(float zoomFactor)
		{
			var zoom = Mathf.Max(0.0f, Mathf.Min(_mapManager.Zoom + zoomFactor * _zoomSpeed, 21.0f));
			if (Math.Abs(zoom - _mapManager.Zoom) > 0.0f)
			{
				_mapManager.UpdateMap(_mapManager.CenterLatitudeLongitude, zoom);
			}
		}

		void PanMapUsingKeyBoard(float xMove, float zMove)
		{
			if (Math.Abs(xMove) > 0.0f || Math.Abs(zMove) > 0.0f)
			{
				// Get the number of degrees in a tile at the current zoom level.
				// Divide it by the tile width in pixels ( 256 in our case)
				// to get degrees represented by each pixel.
				// Keyboard offset is in pixels, therefore multiply the factor with the offset to move the center.
				float factor = _panSpeed * (Conversions.GetTileScaleInDegrees((float)_mapManager.CenterLatitudeLongitude.x, _mapManager.AbsoluteZoom));

				var latitudeLongitude = new Vector2d(_mapManager.CenterLatitudeLongitude.x + zMove * factor * 2.0f, _mapManager.CenterLatitudeLongitude.y + xMove * factor * 4.0f);

				//Debug.Log("LAT LONG: " + latitudeLongitude);
				_mapManager.UpdateMap(latitudeLongitude, _mapManager.Zoom);
			}
		}

		void PanMapUsingTouchOrMouse()
		{
			if (_useDegreeMethod)
			{
				UseDegreeConversion();
			}
			else
			{
				UseMeterConversion();
			}
		}

		void UseMeterConversion()
		{

            if (Input.GetMouseButtonUp(1))
			{
				var mousePosScreen = Input.mousePosition;
				//Debug.Log(mousePosScreen);
				//assign distance of camera to ground plane to z, otherwise ScreenToWorldPoint() will always return the position of the camera
				//http://answers.unity3d.com/answers/599100/view.html
				mousePosScreen.z = _referenceCamera.transform.localPosition.y;
				var pos = _referenceCamera.ScreenToWorldPoint(mousePosScreen);

				var latlongDelta = _mapManager.WorldToGeoPosition(pos);
				Debug.Log("Latitude: " + latlongDelta.x + " Longitude: " + latlongDelta.y);
			}

			if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
			{
				var mousePosScreen = Input.mousePosition;
				//assign distance of camera to ground plane to z, otherwise ScreenToWorldPoint() will always return the position of the camera
				//http://answers.unity3d.com/answers/599100/view.html
				mousePosScreen.z = _referenceCamera.transform.localPosition.y;
				_mousePosition = _referenceCamera.ScreenToWorldPoint(mousePosScreen);

				if (_shouldDrag == false)
				{
					_shouldDrag = true;
					_origin = _referenceCamera.ScreenToWorldPoint(mousePosScreen);
				}
			}
			else
			{
				_shouldDrag = false;
			}

			if (_shouldDrag == true)
			{
				var changeFromPreviousPosition = _mousePositionPrevious - _mousePosition;
				if (Mathf.Abs(changeFromPreviousPosition.x) > 0.0f || Mathf.Abs(changeFromPreviousPosition.y) > 0.0f)
				{
					_mousePositionPrevious = _mousePosition;
					var offset = _origin - _mousePosition;

					if (Mathf.Abs(offset.x) > 0.0f || Mathf.Abs(offset.z) > 0.0f)
					{
						if (null != _mapManager)
						{
							float factor = _panSpeed * Conversions.GetTileScaleInMeters((float)0, _mapManager.AbsoluteZoom) / _mapManager.UnityTileSize;
							var latlongDelta = Conversions.MetersToLatLon(new Vector2d(offset.x * factor, offset.z * factor));
							var newLatLong = _mapManager.CenterLatitudeLongitude + latlongDelta;
							//Debug.Log("NEW LAT LONG: " + newLatLong);

							_mapManager.UpdateMap(newLatLong, _mapManager.Zoom);
						}
					}
					_origin = _mousePosition;
				}
				else
				{
					if (EventSystem.current.IsPointerOverGameObject())
					{
						return;
					}
					_mousePositionPrevious = _mousePosition;
					_origin = _mousePosition;
				}
			}
		}

		void UseDegreeConversion()
		{
			if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
			{
				var mousePosScreen = Input.mousePosition;
				//assign distance of camera to ground plane to z, otherwise ScreenToWorldPoint() will always return the position of the camera
				//http://answers.unity3d.com/answers/599100/view.html
				mousePosScreen.z = _referenceCamera.transform.localPosition.y;
				_mousePosition = _referenceCamera.ScreenToWorldPoint(mousePosScreen);

				if (_shouldDrag == false)
				{
					_shouldDrag = true;
					_origin = _referenceCamera.ScreenToWorldPoint(mousePosScreen);
				}
			}
			else
			{
				_shouldDrag = false;
			}

			if (_shouldDrag == true)
			{
				var changeFromPreviousPosition = _mousePositionPrevious - _mousePosition;
				if (Mathf.Abs(changeFromPreviousPosition.x) > 0.0f || Mathf.Abs(changeFromPreviousPosition.y) > 0.0f)
				{
					_mousePositionPrevious = _mousePosition;
					var offset = _origin - _mousePosition;

					if (Mathf.Abs(offset.x) > 0.0f || Mathf.Abs(offset.z) > 0.0f)
					{
						if (null != _mapManager)
						{
							// Get the number of degrees in a tile at the current zoom level.
							// Divide it by the tile width in pixels ( 256 in our case)
							// to get degrees represented by each pixel.
							// Mouse offset is in pixels, therefore multiply the factor with the offset to move the center.
							float factor = _panSpeed * Conversions.GetTileScaleInDegrees((float)_mapManager.CenterLatitudeLongitude.x, _mapManager.AbsoluteZoom) / _mapManager.UnityTileSize;

							var latitudeLongitude = new Vector2d(_mapManager.CenterLatitudeLongitude.x + offset.z * factor, _mapManager.CenterLatitudeLongitude.y + offset.x * factor);
							_mapManager.UpdateMap(latitudeLongitude, _mapManager.Zoom);
						}
					}
					_origin = _mousePosition;
				}
				else
				{
					if (EventSystem.current.IsPointerOverGameObject())
					{
						return;
					}
					_mousePositionPrevious = _mousePosition;
					_origin = _mousePosition;
				}
			}
		}

		private Vector3 getGroundPlaneHitPoint(Ray ray)
		{
			float distance;
			if (!_groundPlane.Raycast(ray, out distance)) { return Vector3.zero; }
			return ray.GetPoint(distance);
		}
	}
}