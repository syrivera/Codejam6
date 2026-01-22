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

  const weightDifference = progressData.current_weight - progressData.target_weight;
  const weightProgress = weightDifference > 0 ? 'to lose' : 'to gain';

  // Calculate percentages for progress bars
  const calcPercentage = (consumed, target) => Math.min((consumed / target) * 100, 100);
  const calcRemaining = (consumed, target) => Math.max(target - consumed, 0);

  const caloriesPercent = calcPercentage(progressData.consumed_calories, progressData.target_cals);
  const carbsPercent = calcPercentage(progressData.consumed_carbs, progressData.target_carbs);
  const fatPercent = calcPercentage(progressData.consumed_fat, progressData.target_fat);
  const proteinPercent = calcPercentage(progressData.consumed_protein, progressData.target_protein);

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
              <span className="value">{progressData.current_weight} lbs</span>
            </div>
            <div className="weight-item">
              <span className="label">Target</span>
              <span className="value">{progressData.target_weight} lbs</span>
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
            <h3>Today's Nutrition</h3>
          </div>
          <div className="targets-grid">
            <div className="target-item calories">
              <span className="target-label">Calories</span>
              <div className="progress-bar-container">
                <div className="progress-bar" style={{width: `${caloriesPercent}%`}}></div>
              </div>
              <div className="target-values">
                <span className="consumed">{progressData.consumedCalories}</span>
                <span className="separator">/</span>
                <span className="target">{progressData.target_daily_calories} kcal</span>
              </div>
              <span className="remaining">{calcRemaining(progressData.consumed_calories, progressData.target_cals)} kcal left</span>
            </div>
            <div className="target-item protein">
              <span className="target-label">Protein</span>
              <div className="progress-bar-container">
                <div className="progress-bar" style={{width: `${proteinPercent}%`}}></div>
              </div>
              <div className="target-values">
                <span className="consumed">{progressData.consumed_protein}</span>
                <span className="separator">/</span>
                <span className="target">{progressData.target_protein} g</span>
              </div>
              <span className="remaining">{calcRemaining(progressData.consumed_protein, progressData.target_protein)} g left</span>
            </div>
            <div className="target-item carbs">
              <span className="target-label">Carbs</span>
              <div className="progress-bar-container">
                <div className="progress-bar" style={{width: `${carbsPercent}%`}}></div>
              </div>
              <div className="target-values">
                <span className="consumed">{progressData.consumed_carbs}</span>
                <span className="separator">/</span>
                <span className="target">{progressData.target_carbs} g</span>
              </div>
              <span className="remaining">{calcRemaining(progressData.consumed_carbs, progressData.target_carbs)} g left</span>
            </div>
            <div className="target-item fat">
              <span className="target-label">Fat</span>
              <div className="progress-bar-container">
                <div className="progress-bar" style={{width: `${fatPercent}%`}}></div>
              </div>
              <div className="target-values">
                <span className="consumed">{progressData.consumed_fat}</span>
                <span className="separator">/</span>
                <span className="target">{progressData.target_fat} g</span>
              </div>
              <span className="remaining">{calcRemaining(progressData.consumed_fat, progressData.target_fat)} g left</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default ProgressView;
