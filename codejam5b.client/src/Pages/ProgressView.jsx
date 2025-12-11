import { useState, useEffect } from 'react';
import './ProgressView.css';

function ProgressView() {
  const [progressData, setProgressData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchProgressData();
  }, []);

  const fetchProgressData = async () => {
    setIsLoading(true);
    setError(null);

    try {
      const response = await fetch('/api/progress');
      if (!response.ok) {
        throw new Error('Failed to fetch progress data');
      }
      const data = await response.json();
      setProgressData(data);
      
    } catch (err) {
      setError(err.message);
    } finally {
      setIsLoading(false);
    }
  };

  if (isLoading) {
    return (
      <div className="progress-view-container">
        <h2>Progress Stats</h2>
        <div className="loading">Loading...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="progress-view-container">
        <h2>Progress Stats</h2>
        <div className="error-message">
          <p>Error: {error}</p>
          <button onClick={fetchProgressData} className="retry-btn">
            Retry
          </button>
        </div>
      </div>
    );
  }

  if (!progressData) {
    return (
      <div className="progress-view-container">
        <h2>Progress Stats</h2>
        <p>No progress data available.</p>
      </div>
    );
  }

  const weightDifference = progressData.currentWeight - progressData.targetWeight;
  const weightProgress = weightDifference > 0 ? 'to lose' : 'to gain';

  return (
    <div className="progress-view-container">
      <h2>Progress Stats</h2>
      
      <div className="stats-grid">
        <div className="stat-card weight-card">
          <div className="stat-header">
            <h3>Weight</h3>
          </div>
          <div className="weight-stats">
            <div className="weight-item">
              <span className="label">Current</span>
              <span className="value">{progressData.currentWeight} lbs</span>
            </div>
            <div className="weight-item">
              <span className="label">Target</span>
              <span className="value">{progressData.targetWeight} lbs</span>
            </div>
            <div className="weight-progress">
              <span className="progress-text">
                {Math.abs(weightDifference)} lbs {weightProgress}
              </span>
            </div>
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-header">
            <h3>Daily Targets</h3>
          </div>
          <div className="targets-grid">
            <div className="target-item calories">
              <span className="target-label">Calories</span>
              <span className="target-value">{progressData.targetDailyCalories}</span>
              <span className="target-unit">kcal</span>
            </div>
            <div className="target-item protein">
              <span className="target-label">Protein</span>
              <span className="target-value">{progressData.targetDailyProtein}</span>
              <span className="target-unit">g</span>
            </div>
            <div className="target-item carbs">
              <span className="target-label">Carbs</span>
              <span className="target-value">{progressData.targetDailyCarbs}</span>
              <span className="target-unit">g</span>
            </div>
            <div className="target-item fat">
              <span className="target-label">Fat</span>
              <span className="target-value">{progressData.targetDailyFat}</span>
              <span className="target-unit">g</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default ProgressView;
