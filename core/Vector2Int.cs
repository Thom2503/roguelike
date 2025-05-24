using System;
using System.Numerics;

namespace roguelike.core;

public struct Vector2<T> where T : INumber<T> {
	public T x;
	public T y;

	public Vector2(T x, T y) {
		this.x = x;
		this.y = y;
	}

	public static bool operator ==(Vector2<T> a, Vector2<T> b) => a.x == b.x && a.y == b.y;

	public static bool operator !=(Vector2<T> a, Vector2<T> b) => !(a == b);

	public override bool Equals(object obj) => obj is Vector2<T> other && this == other;

	public override int GetHashCode() => HashCode.Combine(x, y);
}